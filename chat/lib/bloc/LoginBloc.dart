import "dart:convert";

import "package:bloc/bloc.dart";
import "LoginState.dart";
import "LoginEvent.dart";
import "package:http/http.dart" as http;
import "../Config/ApiConfig.dart";

//Login bloc is used to handle login events and states.
class LoginBloc extends Bloc<LoginEvent, LoginState> 
{
    LoginBloc() : super(LoginInitial())
    {
        on<LoginButtonPressed>((event, emit) async 
        {
            try
            {
                //Tell the GUI we are busy.
                emit(LoginLoading());
                
                //Todo: Turn into a member function. I also need to grab and set the access and refresh tokens.
                var loginEndpoint = Uri.parse("${ApiConfig.ApiV1BaseUrl}/users/login");
                var userLoginReq = UserLoginRequest(UserIdentifier: event.UserIdentifier, Password: event.Password);

                var response = await http.post(loginEndpoint,
                headers: {"Content-Type": "application/json"},
                 body: jsonEncode(userLoginReq.toJson()));

                //If the login worked, welcome.
                if (response.statusCode == 200)
                {
                    //Login successful
                    emit(LoginSuccessful("Login successful."));
                }
                else if (response.statusCode == 401)
                {
                    emit(LoginFailure("Unauthorised login attempt blocked."));
                }
                else if(response.statusCode == 404)
                {
                    emit(LoginFailure("API Endpoint responsible for handling this function could not be found."));
                }
                else
                {
                    //Login failed
                    emit(LoginFailure("Login failed. Please try again."));
                }
            }
            //Handle any exceptions during login
            catch (ex)
            {
                emit(LoginFailure("Something went wrong during login, please try again.\n${ex}"));
            }
        });  
    }
}

//DTO's for the login bloc.
class UserLoginRequest 
{
    final String UserIdentifier;
    final String Password;

    //constructor
    UserLoginRequest(
    {
        required this.UserIdentifier,
        required this.Password
    });

    //Represent this as a key-value map
    Map<String, dynamic> toJson() => {
        "userIdentifier": UserIdentifier,
        "password": Password
    };
}