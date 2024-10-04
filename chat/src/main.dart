import "package:flutter/material.dart";

import "MainUi/LoginScreen.dart";
import "MainUi/MainScreen.dart";

//Using C# formatting rules since it's a ton easier to follow.

//In C#: this is always static void Main(string[] args) or static void Main(). It can be public or possibly private. 
void main() 
{
    runApp(ChatApp());
}

//ChatApp is the main class of this app, which is why I have left it in main.dart.
class ChatApp extends StatelessWidget
{
    bool IsLoggedIn = false;

    @override
    Widget build(BuildContext context)
    {
        return MaterialApp(
            title:"Chat App", 
            theme: ThemeData(colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple), useMaterial3: false), 
            home: Scaffold(
                //If is logged in is true, show the home screen else show the login screen.
                body: IsLoggedIn == true ? MainScreen() : LoginScreen()
            )
        );
    }
}