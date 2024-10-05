//This is the main screen of the app.

import 'package:flutter/material.dart';

class MainScreen extends StatelessWidget
{
    @override
    Widget build(BuildContext context)
    {
        return Padding(
          padding: const EdgeInsets.all(8.0),
          child: ListView(
            children: [
              Text("Main Screen"),
            ],
          ),
        );
    }
}