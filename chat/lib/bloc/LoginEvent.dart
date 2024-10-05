//LoginEvent.dart - manages the login events of the app.
import "package:equatable/equatable.dart";

abstract class LoginEvent extends Equatable
{
    @override
    List<Object> get props => [];
}

class LoginButtonPressed extends LoginEvent
{
    final String UserIdentifier;
    final String Password;

    LoginButtonPressed({required this.UserIdentifier, required this.Password});

    @override
    List<Object> get props => [UserIdentifier, Password];
}

class LogoffButtonPressed extends LoginEvent
{
    
}

class ForgotPasswordButtonPressed extends LoginEvent
{
    
}