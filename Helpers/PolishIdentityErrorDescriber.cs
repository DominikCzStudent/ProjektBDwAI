using Microsoft.AspNetCore.Identity;

namespace Projekt.Helpers
{
	public class PolishIdentityErrorDescriber : IdentityErrorDescriber
	{
		// Użytkownik

		public override IdentityError DuplicateUserName(string userName)
			=> new IdentityError
			{
				Code = nameof(DuplicateUserName),
				Description = $"Użytkownik o nazwie „{userName}” już istnieje."
			};

		public override IdentityError InvalidUserName(string userName)
			=> new IdentityError
			{
				Code = nameof(InvalidUserName),
				Description = $"Nazwa użytkownika „{userName}” jest nieprawidłowa."
			};

		public override IdentityError UserAlreadyHasPassword()
			=> new IdentityError
			{
				Code = nameof(UserAlreadyHasPassword),
				Description = "Użytkownik ma już ustawione hasło."
			};

		public override IdentityError UserLockoutNotEnabled()
			=> new IdentityError
			{
				Code = nameof(UserLockoutNotEnabled),
				Description = "Blokada konta nie została włączona."
			};

		public override IdentityError UserAlreadyInRole(string role)
			=> new IdentityError
			{
				Code = nameof(UserAlreadyInRole),
				Description = $"Użytkownik jest już w roli „{role}”."
			};

		public override IdentityError UserNotInRole(string role)
			=> new IdentityError
			{
				Code = nameof(UserNotInRole),
				Description = $"Użytkownik nie należy do roli „{role}”."
			};

		// Hasła

		public override IdentityError PasswordTooShort(int length)
			=> new IdentityError
			{
				Code = nameof(PasswordTooShort),
				Description = $"Hasło musi mieć co najmniej {length} znaków."
			};

		public override IdentityError PasswordRequiresDigit()
			=> new IdentityError
			{
				Code = nameof(PasswordRequiresDigit),
				Description = "Hasło musi zawierać co najmniej jedną cyfrę (0–9)."
			};

		public override IdentityError PasswordRequiresUpper()
			=> new IdentityError
			{
				Code = nameof(PasswordRequiresUpper),
				Description = "Hasło musi zawierać co najmniej jedną wielką literę (A–Z)."
			};

		public override IdentityError PasswordRequiresLower()
			=> new IdentityError
			{
				Code = nameof(PasswordRequiresLower),
				Description = "Hasło musi zawierać co najmniej jedną małą literę (a–z)."
			};

		public override IdentityError PasswordRequiresNonAlphanumeric()
			=> new IdentityError
			{
				Code = nameof(PasswordRequiresNonAlphanumeric),
				Description = "Hasło musi zawierać co najmniej jeden znak specjalny."
			};

		// Role

		public override IdentityError DuplicateRoleName(string role)
			=> new IdentityError
			{
				Code = nameof(DuplicateRoleName),
				Description = $"Rola „{role}” już istnieje."
			};

		public override IdentityError InvalidRoleName(string role)
			=> new IdentityError
			{
				Code = nameof(InvalidRoleName),
				Description = $"Nazwa roli „{role}” jest nieprawidłowa."
			};

		// Tokeny/Autoryzacja

		public override IdentityError InvalidToken()
			=> new IdentityError
			{
				Code = nameof(InvalidToken),
				Description = "Token jest nieprawidłowy lub wygasł."
			};

		public override IdentityError RecoveryCodeRedemptionFailed()
			=> new IdentityError
			{
				Code = nameof(RecoveryCodeRedemptionFailed),
				Description = "Nieprawidłowy kod odzyskiwania."
			};

		// E-mail

		public override IdentityError InvalidEmail(string email)
			=> new IdentityError
			{
				Code = nameof(InvalidEmail),
				Description = $"Adres e-mail „{email}” jest nieprawidłowy."
			};

		public override IdentityError DuplicateEmail(string email)
			=> new IdentityError
			{
				Code = nameof(DuplicateEmail),
				Description = $"Adres e-mail „{email}” jest już zajęty."
			};
	}
}
