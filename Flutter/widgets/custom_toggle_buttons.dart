import 'package:flutter/material.dart';

class CustomToggleButtons extends StatelessWidget {
  final List<bool> isSelected;
  final void Function(int index) onPressed;

  const CustomToggleButtons({super.key, 
    required this.isSelected,
    required this.onPressed,
  });
  @override
  Widget build(BuildContext context) {
    return ToggleButtons(
      isSelected: isSelected,
      constraints: const BoxConstraints(minHeight: 26, minWidth: 41),
      borderRadius: BorderRadius.circular(8),
      borderColor: const Color.fromARGB(255, 117, 199, 100),
      selectedBorderColor: const Color.fromARGB(255, 117, 199, 100),
      disabledBorderColor: const Color.fromARGB(255, 117, 199, 100),
      color: const Color.fromARGB(255, 117, 199, 100),
      selectedColor: Colors.white,
      fillColor: const Color.fromARGB(255, 117, 199, 100),
      onPressed: onPressed,
      //focusColor: Color.fromARGB(255, 117, 199, 100),
      children:  const [
        Icon(
          Icons.view_agenda_outlined,
          size: 17,
        ),
        Icon(
          Icons.grid_view_outlined,
          size: 17,
        )
      ],
    );
  }
}
