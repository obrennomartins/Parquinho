namespace Stickers.Models.Exceptions;

// TODO Throwing exception is expensive; try Result Pattern instead

public class UnauthorizedException(string message) : Exception(message) { }

public class ForbiddenException(string message) : Exception(message) { }

public class UsernameAlreadyExistsException(string message) : Exception(message) { }

public class IdentityRegistrationException(string message) : Exception(message) { }

public class ConflictException(string message) : Exception(message) { }

public class NotFoundException(string message) : Exception(message) { }
