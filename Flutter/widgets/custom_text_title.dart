import 'package:flutter/material.dart';

class CustomTextTitle extends StatelessWidget {
  final String title;
  final FontWeight fontWeight;

  const CustomTextTitle({super.key, required this.title,required this.fontWeight});
  @override
  Widget build(BuildContext context) {
    return Text(
      title,
      style: TextStyle(
        fontSize: 20,
        fontWeight: fontWeight,
      ),
    );
  }
}
