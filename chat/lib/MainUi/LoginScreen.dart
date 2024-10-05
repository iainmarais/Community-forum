//This is the login screen of the app.

import 'package:ChatApp/MainUi/MainScreen.dart';
import 'package:ChatApp/bloc/LoginState.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'package:ChatApp/bloc/LoginBloc.dart';
import 'package:ChatApp/bloc/LoginEvent.dart';

class LoginScreen extends StatefulWidget 
{
  const LoginScreen({Key? key}) : super(key: key);
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> 
{
    TextEditingController userIdentifierController = TextEditingController();
    TextEditingController passwordController = TextEditingController();

  void LoginUser() 
  {
    //Use the Login Bloc to handle this.
    BlocProvider.of<LoginBloc>(context).add(LoginButtonPressed(
        UserIdentifier: userIdentifierController.text,
        Password: passwordController.text));
  }

  void ResetPassword() 
  {
    //Todo: Create a bloc event to handle this.
  }

  @override
  Widget build(BuildContext context) {
    return BlocConsumer<LoginBloc, LoginState>(
      listener: (context, state) 
      {
          if (state is LoginSuccessful) 
          {
              //Login successful. Navigate to the main screen.
              Navigator.pushReplacement(
                  context, MaterialPageRoute(builder: (context) => MainScreen()));
          } 
        else if (state is LoginLoading) 
          {
              showDialog(
                  context: context,
                  barrierDismissible: false,
                  builder: (BuildContext context) 
                  {
                      return const Center(child: CircularProgressIndicator());
                  }
              );
          } 
          else if (state is LoginFailure) 
          {
              //Login failed. Show an error message.
              ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
              content: Text('Login failed. Please try again.')));
          }
      },
      builder: (context, state) {
        return Padding(
          padding: const EdgeInsets.all(10),
          child: ListView(children: [
            Container(
              alignment: Alignment.center,
              padding: const EdgeInsets.all(10),
              child: const Text(
                'Community Chat',
                style: TextStyle(
                    color: Colors.blue,
                    fontWeight: FontWeight.w500,
                    fontSize: 30),
              ),
            ),
            Container(
              alignment: Alignment.center,
              padding: const EdgeInsets.all(10),
              child: const Text(
                'Log in',
                style: TextStyle(fontSize: 20),
              ),
            ),
            //Text input fields for user identifier (username or email address) and password.
            Container(
              padding: const EdgeInsets.all(10),
              child: TextField(
                controller: userIdentifierController,
                decoration: const InputDecoration(
                    border: OutlineInputBorder(),
                    labelText: 'Email address or Username'),
              ),
            ),
            Container(
              padding: const EdgeInsets.fromLTRB(10, 10, 10, 0),
              child: TextField(
                controller: passwordController,
                obscureText: true,
                decoration: const InputDecoration(
                    border: OutlineInputBorder(), labelText: 'Password'),
              ),
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                ElevatedButton.icon(
                    onPressed: () => ResetPassword(),
                    icon: const Icon(Icons.person),
                    label: const Text('Forgot Password?')),
                ElevatedButton.icon(
                    onPressed: () => LoginUser(),
                    icon: const Icon(Icons.login),
                    label: const Text('Log in')),
              ],
            )
          ]),
        );
      },
    );
  }
}
