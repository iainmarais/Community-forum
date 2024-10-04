//LoginState.dart - manages the login state of the app.
import "package:equatable/equatable.dart";

abstract class LoginState extends Equatable
{
    @override
    //Observation: In C# we can do this now: List<string> MyStrings [], though it does have some drawbacks in huge apps. Best to keep to List<obj> ColName = new() or var ColName = new List<obj>();
    List<Object> get props => [];
}

class LoginInitial extends LoginState
{

}

class LoginLoading extends LoginState
{

}

class LoginSuccessful extends LoginState
{

}

//If the login failed
class LoginFailure extends LoginState
{
    final String err;

    LoginFailure(this.err);

    @override
    List<Object> get props => [err];
}

//When the user logs off

class LogoffSuccessful extends LoginState
{

}