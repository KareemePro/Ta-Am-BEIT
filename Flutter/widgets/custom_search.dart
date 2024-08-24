import 'package:flutter/material.dart';

class CustomSearch extends StatelessWidget {

  const CustomSearch({super.key});
  @override
  Widget build(BuildContext context) {
    return TextFormField(
      decoration: InputDecoration(
        prefixIcon: const Icon(
          Icons.search,
          color: Color.fromARGB(255, 101, 101, 101),
          size: 28,
        ),
        hintText: 'Chef name or Dish',
        fillColor: const Color.fromARGB(255, 244, 244, 244),
        filled: true,
        isDense: true,
        border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: BorderSide.none),
      ),
    );
  }
}
