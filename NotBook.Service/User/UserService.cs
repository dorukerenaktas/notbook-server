using System;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Http;
using NotBook.Core.Entity;
using NotBook.Data.Constants;
using NotBook.Data.MicroOrm;
using NotBook.Data.Repository;
using NotBook.Service.Common;
using NotBook.Service.Common.DTOs;
using NotBook.Service.Email;
using NotBook.Service.Hash;
using NotBook.Service.University.DTOs;
using NotBook.Service.User.DTOs;
using NotBook.Service.User.Exception;

namespace NotBook.Service.User
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly IHashService _hashService;
        private readonly IImageService _imageService;
        private readonly IFileService _fileService;
        private readonly IMicroOrmRepository _microOrmRepository;
        private readonly IRepository<UniversityEntity> _universityRepository;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<VerificationEntity> _verificationRepository;

        public UserService(IHashService hashService,
            IEmailService emailService,
            IImageService imageService,
            IFileService fileService,
            IMicroOrmRepository microOrmRepository,
            IRepository<UserEntity> userRepository,
            IRepository<UniversityEntity> universityRepository,
            IRepository<VerificationEntity> verificationRepository)
        {
            _emailService = emailService;
            _hashService = hashService;
            _imageService = imageService;
            _fileService = fileService;
            _microOrmRepository = microOrmRepository;
            _userRepository = userRepository;
            _verificationRepository = verificationRepository;
            _universityRepository = universityRepository;
        }

        public UserDto Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _userRepository.Table.FirstOrDefault(x => x.UserEmail == email);

            if (user == null)
                throw new UserNotFoundException();

            if (!_hashService.VerifyHash(password, user.UserPasswordHash, user.UserPasswordSalt))
                throw new UserNotFoundException();

            return Read(user.UserIdPk);
        }

        public void Create(string firstName, string lastName, string email, string password)
        {
            var university = _universityRepository.Table.FirstOrDefault(x =>
                x.UniversityMailExtension == email.Split('@', StringSplitOptions.None)[1]);

            if (university == null)
                throw new InvalidStudentMailException();
            if (_userRepository.Table.Any(x => x.UserEmail == email))
                throw new EmailIsTakenException();

            _hashService.CreateHash(password, out var passwordHash, out var passwordSalt);

            var verificationHash = Guid.NewGuid().ToString();

            var user = new UserEntity
            {
                UserFirstName = firstName,
                UserLastName = lastName,
                UserEmail = email,
                UserUniversityIdFk = university.UniversityIdPk,
                UserPasswordHash = passwordHash,
                UserPasswordSalt = passwordSalt
            };

            _userRepository.Insert(user);
            _userRepository.SaveAll();

            _verificationRepository.Insert(new VerificationEntity
            {
                VerificationUserIdFk = user.UserIdPk,
                VerificationHash = verificationHash,
                VerificationType = VerificationType.Email
            });
            _verificationRepository.SaveAll();

            _emailService.SendVerificationEmail(email, verificationHash);
        }

        public UserDto Read(int id)
        {
            const string sql =
                "SELECT UserIdPk, UserFirstName, UserLastName, UserIsVerified," +
                "(SELECT COUNT(*) FROM Attend WHERE AttendUserIdFk = @userId) AS UserAttendedLectureCount," +
                "(SELECT COUNT(*) FROM Lecture WHERE LectureCreatedByIdFk = @userId) AS UserAddedLectureCount," +
                "(SELECT COUNT(*) FROM NoteFav WHERE NoteFavUserIdFk = @userId) AS UserFavNoteCount," +
                "(SELECT COUNT(*) FROM Note WHERE NoteCreatedByIdFk = @userId) AS UserAddedNoteCount," +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl " +
                "FROM User INNER JOIN University ON UserUniversityIdFk = UniversityIdPk " +
                "WHERE UserIdPk = @userId ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", id);
            var result = _microOrmRepository.Connection.Query<ExtendedUserEntity, UniversityEntity, UserDto>(sql,
                (user, university) =>
                {
                    // Control email verification
                    if (!user.UserIsVerified) throw new UserNotVerifiedException();

                    return new UserDto
                    {
                        Id = user.UserIdPk,
                        FirstName = user.UserFirstName,
                        LastName = user.UserLastName,
                        AttendedLectureCount = user.UserAttendedLectureCount,
                        AddedLectureCount = user.UserAddedLectureCount,
                        FavNoteCount = user.UserFavNoteCount,
                        AddedNoteCount = user.UserAddedNoteCount,
                        University = new SimpleUniversityDto
                        {
                            Id = university.UniversityIdPk,
                            Name = university.UniversityName,
                            Abbr = university.UniversityAbbr,
                            ImageUrl = university.UniversityImageUrl
                        }
                    };
                },
                dynamicParameters,
                splitOn: "UniversityIdPk").FirstOrDefault();

            if (result == null)
                throw new UserNotFoundException();

            return result;
        }

        public void Update(int id, string firstName, string lastName, string password)
        {
            var user = _userRepository.Table.FirstOrDefault(x => x.UserIdPk == id);

            if (user == null)
                throw new ArgumentException("User not found");

            // update user properties
            user.UserFirstName = firstName;
            user.UserLastName = lastName;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                _hashService.CreateHash(password, out var passwordHash, out var passwordSalt);

                user.UserPasswordHash = passwordHash;
                user.UserPasswordSalt = passwordSalt;
            }

            _userRepository.SaveAll();
        }
        
        public FileDto ReadImage(int userId)
        {
            var user = _userRepository.Table.FirstOrDefault(x => x.UserIdPk == userId);
            if (user == null)
                throw new UserNotFoundException();
            
            var stream = _fileService.Get(DataConstants.AvatarFilePath, user.UserImageName);
            return new FileDto
            {
                FileName = Path.GetRandomFileName(),
                FileExtension = Path.GetExtension(user.UserImageName),
                FileStream = stream
            };
        }
        
        public void UpdateImage(IFormFile file, int userId)
        {
            var image = _imageService.Resize(file);
            var fileName = _fileService.GenerateFileName(image, userId.ToString());
            _fileService.Save(image, DataConstants.AvatarFilePath, fileName);

            var user = _userRepository.Table.FirstOrDefault(x => x.UserIdPk == userId);

            if (user == null)
                throw new ArgumentException("User not found");

            // update user properties
            user.UserImageName = fileName;
            _userRepository.SaveAll();
        }

        public void VerifyEmail(string verificationHash)
        {
            var verification =
                _verificationRepository.Table.FirstOrDefault(x =>
                    x.VerificationHash == verificationHash && x.VerificationType == VerificationType.Email);

            if (verification == null)
                throw new UserNotFoundException();

            var user = _userRepository.Table.FirstOrDefault(x => x.UserIdPk == verification.VerificationUserIdFk);

            if (user == null)
                throw new UserNotFoundException();

            user.UserIsVerified = true;
            _userRepository.SaveAll();

            _verificationRepository.Delete(verification);
            _verificationRepository.SaveAll();
        }

        public void ResendEmailVerificationEmail(string email)
        {
            var user = _userRepository.Table.FirstOrDefault(x => x.UserEmail == email);
            if (user == null)
                throw new UserNotFoundException();

            var verificationHash = Guid.NewGuid().ToString();

            var verification = _verificationRepository.Table.FirstOrDefault(x => 
                x.VerificationUserIdFk == user.UserIdPk && x.VerificationType == VerificationType.Email);
            
            if (verification == null)
            {
                verification = new VerificationEntity
                {
                    VerificationUserIdFk = user.UserIdPk,
                    VerificationHash = verificationHash,
                    VerificationType = VerificationType.Email
                };
                _verificationRepository.Insert(verification);
            }
            else
            {
                verification.VerificationHash = verificationHash;
            }

            _verificationRepository.SaveAll();
            _emailService.SendVerificationEmail(email, verificationHash);
        }
        
        public void VerifyPassword(string verificationHash, string password)
        {
            var verification =
                _verificationRepository.Table.FirstOrDefault(x =>
                    x.VerificationHash == verificationHash && x.VerificationType == VerificationType.Password);
            if (verification == null)
                throw new UserNotFoundException();

            var user = _userRepository.Table.FirstOrDefault(x => x.UserIdPk == verification.VerificationUserIdFk);
            if (user == null)
                throw new UserNotFoundException();

            _hashService.CreateHash(password, out var passwordHash, out var passwordSalt);
            user.UserPasswordHash = passwordHash;
            user.UserPasswordSalt = passwordSalt;
            _userRepository.SaveAll();

            _verificationRepository.Delete(verification);
            _verificationRepository.SaveAll();
        }

        public void SendForgotPasswordEmail(string email)
        {
            var user = _userRepository.Table.FirstOrDefault(x => x.UserEmail == email);
            if (user == null)
                throw new UserNotFoundException();

            var verificationHash = Guid.NewGuid().ToString();

            var verification = _verificationRepository.Table.FirstOrDefault(x =>
                x.VerificationUserIdFk == user.UserIdPk && x.VerificationType == VerificationType.Password);
            if (verification == null)
            {
                verification = new VerificationEntity
                {
                    VerificationUserIdFk = user.UserIdPk,
                    VerificationHash = verificationHash,
                    VerificationType = VerificationType.Password
                };
                _verificationRepository.Insert(verification);
            }
            else
            {
                verification.VerificationHash = verificationHash;
            }

            _verificationRepository.SaveAll();
            _emailService.SendForgotPasswordEmail(email, verificationHash);
        }
    }
}