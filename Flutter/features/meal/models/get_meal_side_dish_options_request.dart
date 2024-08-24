class GetMealSideDishOptionsRequest {
  final String? sideDishID;
  final int? sideDishSizeOption;
  final String? name;
  final int? price;
  final int? quantity;

  GetMealSideDishOptionsRequest(
      {this.sideDishID,
      this.sideDishSizeOption,
      this.name,
      this.price,
      this.quantity});

  factory GetMealSideDishOptionsRequest.fromJson(Map<String, dynamic> json) {
    return GetMealSideDishOptionsRequest(
      sideDishID:  json['sideDishID'],
      sideDishSizeOption: json['sideDishSizeOption'],
      name: json['name'],
      price: json['price'],
      quantity:json['quantity'], 
    );
  
  
  
  }
}
