
import 'package:flutter/material.dart';

class CustomCircularLoding extends StatelessWidget {
  const CustomCircularLoding({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return const SizedBox(
      height: 300,
      child: Center(
        child: CircularProgressIndicator(
          color: Color.fromARGB(255, 42, 145, 21),
        ),
      ),
    );
  }
}
