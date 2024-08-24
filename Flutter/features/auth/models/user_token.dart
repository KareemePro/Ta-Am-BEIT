class UserToken {
  final dynamic roles;
  final String jwt;

  UserToken({required this.roles, required this.jwt});

  factory UserToken.fromJson(Map<String, dynamic> json) {
    return UserToken(roles: json['roles'], jwt: json['jwt']);
  }
}
