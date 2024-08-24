import 'package:flutter/material.dart';

class CustomIconBackground extends StatelessWidget {
  final void Function() onTap;
  final Icon icon;
  const CustomIconBackground({super.key, required this.onTap, required this.icon});

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: onTap,
      child: CircleAvatar(
        radius: 13,
        backgroundColor: Colors.white,
        child: icon,
      ),
    );
  }
}
