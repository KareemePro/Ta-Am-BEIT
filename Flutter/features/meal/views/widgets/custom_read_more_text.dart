import 'package:flutter/material.dart';
import 'package:readmore/readmore.dart';

class CustomReadMoreText extends StatelessWidget {
  final String text;
  const CustomReadMoreText({super.key,required this.text});

  @override
  Widget build(BuildContext context) {
    return ReadMoreText(
      text,
      trimLines: 3,
      textAlign: TextAlign.start,
      trimMode: TrimMode.Line,
      trimCollapsedText: 'Read More',
      trimExpandedText: 'Read Less',
      lessStyle: const TextStyle(
        fontSize: 12,
        fontWeight: FontWeight.w400,
        color: Color.fromARGB(255, 101, 192, 85),
      ),
      moreStyle: const TextStyle(
        fontSize: 12,
        fontWeight: FontWeight.w400,
        color: Color.fromARGB(255, 101, 192, 85),
      ),
      style: const TextStyle(
        fontSize: 13,
        fontWeight: FontWeight.w400,
        color: Color.fromARGB(255, 101, 101, 101),
      ),
    );
  }
}
