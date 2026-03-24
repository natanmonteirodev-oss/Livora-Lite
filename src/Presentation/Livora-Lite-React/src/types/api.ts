export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface AuthResponse {
  success: boolean;
  message?: string;
  user?: UserDTO;
  token?: string;
}

export interface UserDTO {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
}

export interface PropertyDTO {
  id: number;
  address: string;
  propertyType: string;
  status: string;
  details?: string;
}

export interface TenantDTO {
  id: number;
  name: string;
  email: string;
  phone: string;
}

export interface ContractDTO {
  id: number;
  startDate: string;
  endDate: string;
  rentalAmount: number;
  status: string;
}

export interface BillingDTO {
  id: number;
  amount: number;
  dueDate: string;
  status: string;
}

export interface PaymentDTO {
  id: number;
  amount: number;
  paymentDate: string;
  method: string;
}

export interface MaintenanceRequestDTO {
  id: number;
  description: string;
  priority: string;
  status: string;
  createdDate: string;
}

export interface DashboardDTO {
  totalProperties: number;
  totalTenants: number;
  totalIncome: number;
  pendingPayments: number;
}
