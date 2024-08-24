import 'package:flutter/material.dart';

class CustomIconRating extends StatelessWidget {
  const CustomIconRating({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return const Icon(Icons.star_rounded,color: Color.fromARGB(255,250, 130, 50),size: 19,);
  }
}