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

    LoginButtonPressed(this.UserIdentifier, this.Password);

    @override
    List<Object> get props => [UserIdentifier, Password];
}

class Logoff extends LoginEvent
{
    //Handle our logoff logic here.
}