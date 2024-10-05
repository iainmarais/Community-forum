import "package:ChatApp/bloc/LoginBloc.dart";
import "package:ChatApp/bloc/LoginState.dart";
import "package:flutter/material.dart";
import "package:flutter_bloc/flutter_bloc.dart";

import "MainUi/LoginScreen.dart";
import "MainUi/MainScreen.dart";

//Using C# formatting rules since it's a ton easier to follow.

//In C#: this is always static void Main(string[] args) or static void Main(). It can be public or possibly private.
void main() {
  runApp(BlocProvider(
    create: (context) => LoginBloc(),
    child: ChatApp(),
  ));
}

//ChatApp is the main class of this app, which is why I have left it in main.dart.
class ChatApp extends StatelessWidget {
  bool IsLoggedIn = false;

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
        title: "Chat App",
        theme: ThemeData(
            colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
            useMaterial3: false),
        home: BlocBuilder<LoginBloc, LoginState>(
          builder: (context, state) {
            return Scaffold(
                appBar: AppBar(title: Text("Community chat")),
                //If is logged in is true, show the home screen else show the login screen.
                body:
                    (state is LoginSuccessful) ? MainScreen() : LoginScreen());
          },
        ));
  }
}
