export class AppConstants {
  public static readonly API_BASE_URL = 'https://localhost:44348/api';
  public static readonly TIMEOUT = 5000; // in milliseconds
  public static readonly CUSTOMERS_ENDPOINT = AppConstants.API_BASE_URL + '/customers';
  public static readonly APPOINTENTS_ENDPOINT = AppConstants.API_BASE_URL + '/appointments';
}
