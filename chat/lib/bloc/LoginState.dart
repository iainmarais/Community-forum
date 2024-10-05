//LoginState.dart - manages the login state of the app.
import "package:equatable/equatable.dart";
import "package:flutter/material.dart";

@immutable
abstract class LoginState extends Equatable
{
    @override
    List<Object> get props => [];

    String message = "";
}

class LoginInitial extends LoginState
{
    LoginInitial()
    {
        message = "";
    }
}

class LoginLoading extends LoginState
{
    LoginLoading()
    {
        message = "";
    }
}

class LoginSuccessful extends LoginState
{
    LoginSuccessful(String successMessage)
    {
        message = successMessage;
    }
}

//If the login failed
class LoginFailure extends LoginState 
{
    LoginFailure(String failureMessage)
    {
        message = failureMessage;
    }
}