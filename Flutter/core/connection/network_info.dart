import 'package:data_connection_checker_tv/data_connection_checker.dart';

abstract class NetworkInfo {
  Future<bool>? get isConnected;
}

class NetworkInfoImplements implements NetworkInfo {
  final DataConnectionChecker connectionchecker;

  NetworkInfoImplements(this.connectionchecker);

  @override
  Future<bool> get isConnected => connectionchecker.hasConnection;
}
