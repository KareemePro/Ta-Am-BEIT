import 'package:flutter/material.dart';

class TextFormFieldLogin extends StatelessWidget {
  final String labelText;
  final IconData? icon;
  final Color? iconColor;
  final double? iconSize;
  final bool? scureText;
  final FormFieldValidator? validator;
  final void Function(String?)?  onSaved;

  const TextFormFieldLogin({
    super.key,
    required this.labelText,
    this.icon,
    this.iconColor,
    this.iconSize,
    this.scureText = false,
    this.validator,
    this.onSaved,
  });

  @override
  Widget build(BuildContext context) {
    return TextFormField(
      autovalidateMode: AutovalidateMode.onUserInteraction,
      obscureText: scureText!,
      keyboardType: TextInputType.visiblePassword,
      
      decoration: InputDecoration(
        border: const OutlineInputBorder(
          borderRadius: BorderRadius.all(Radius.circular(10)),
          borderSide:
              BorderSide(color: Color.fromARGB(255, 139, 139, 139), width: 2),
        ),
        contentPadding: const EdgeInsets.symmetric(vertical: 0, horizontal: 10),
        label: Text(
          labelText,
          style: const TextStyle(
            color: Color.fromARGB(255, 139, 139, 139),
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w400,
            fontSize: 17,
          ),
        ),
        suffixIcon: Icon(
          icon,
          color: iconColor,
          size: iconSize,
        ),
        focusedBorder: const OutlineInputBorder(
          borderSide:
              BorderSide(color: Color.fromARGB(255, 49, 172, 24), width: 2),
          borderRadius: BorderRadius.all(Radius.circular(10)),
        ),
      ),
      validator: validator,
      onSaved: onSaved,
    );
  }
}
