import 'package:flutter/material.dart';

class CustomLiteButton extends StatelessWidget {
  final String textName;
  final VoidCallback onPressed;

  const CustomLiteButton({
    required this.textName,
    required this.onPressed,
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    double width = MediaQuery.of(context).size.width;
    return MaterialButton(
      shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(10),
          side: const BorderSide(color: Color.fromARGB(255, 42, 145, 21))),
      color: const Color.fromARGB(255, 252, 255, 251),
      textColor: const Color.fromARGB(255, 42, 145, 21),
      minWidth: width,
      height: 50,
      onPressed: onPressed,
      child: Text(
        textName,
        style: const TextStyle(
          //fontFamily: 'Poppins',
          fontWeight: FontWeight.w400,
          fontSize: 20,
        ),
      ),
    );
  }
}
