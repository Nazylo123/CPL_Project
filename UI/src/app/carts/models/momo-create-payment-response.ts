export interface MomoCreatePaymentResponse {
  requestId: string; // Tương ứng với RequestId
  errorCode: number; // Tương ứng với ErrorCode
  orderId: string; // Tương ứng với OrderId
  message: string; // Tương ứng với Message
  localMessage: string; // Tương ứng với LocalMessage
  requestType: string; // Tương ứng với RequestType
  payUrl: string; // Tương ứng với PayUrl
  signature: string; // Tương ứng với Signature
  qrCodeUrl: string; // Tương ứng với QrCodeUrl
  deeplink: string; // Tương ứng với Deeplink
  deeplinkWebInApp: string; // Tương ứng với DeeplinkWebInApp
}
