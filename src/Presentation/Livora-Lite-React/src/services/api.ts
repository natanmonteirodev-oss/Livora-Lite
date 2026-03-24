import axios, { AxiosInstance } from 'axios';
import { 
  LoginRequest, 
  RegisterRequest, 
  AuthResponse,
  UserDTO,
  PropertyDTO,
  TenantDTO,
  ContractDTO,
  BillingDTO,
  PaymentDTO,
  MaintenanceRequestDTO,
  DashboardDTO
} from '../types/api';

const API_BASE_URL = 'http://localhost:5145/api';

class ApiService {
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Interceptor para adicionar token JWT
    this.api.interceptors.request.use((config) => {
      const token = localStorage.getItem('authToken');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });

    // Interceptor para tratar erros de autenticação
    this.api.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          localStorage.removeItem('authToken');
          localStorage.removeItem('user');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // === AUTH ===
  async login(request: LoginRequest): Promise<AuthResponse> {
    try {
      const { data } = await this.api.post<AuthResponse>('/Auth/login', request);
      if (data.token) {
        localStorage.setItem('authToken', data.token);
        if (data.user) {
          localStorage.setItem('user', JSON.stringify(data.user));
        }
      }
      return data;
    } catch (error) {
      throw error;
    }
  }

  async register(request: RegisterRequest): Promise<AuthResponse> {
    try {
      const { data } = await this.api.post<AuthResponse>('/Auth/register', request);
      if (data.token) {
        localStorage.setItem('authToken', data.token);
        if (data.user) {
          localStorage.setItem('user', JSON.stringify(data.user));
        }
      }
      return data;
    } catch (error) {
      throw error;
    }
  }

  // === USERS ===
  async getAllUsers(): Promise<UserDTO[]> {
    const { data } = await this.api.get<UserDTO[]>('/Users');
    return data;
  }

  async getUserById(id: number): Promise<UserDTO> {
    const { data } = await this.api.get<UserDTO>(`/Users/${id}`);
    return data;
  }

  async updateUser(id: number, user: Partial<UserDTO>): Promise<void> {
    await this.api.put(`/Users/${id}`, user);
  }

  async deleteUser(id: number): Promise<void> {
    await this.api.delete(`/Users/${id}`);
  }

  // === PROPERTIES ===
  async getAllProperties(): Promise<PropertyDTO[]> {
    const { data } = await this.api.get<PropertyDTO[]>('/Properties');
    return data;
  }

  async getPropertyById(id: number): Promise<PropertyDTO> {
    const { data } = await this.api.get<PropertyDTO>(`/Properties/${id}`);
    return data;
  }

  async createProperty(property: Partial<PropertyDTO>): Promise<PropertyDTO> {
    const { data } = await this.api.post<PropertyDTO>('/Properties', property);
    return data;
  }

  async updateProperty(id: number, property: Partial<PropertyDTO>): Promise<void> {
    await this.api.put(`/Properties/${id}`, property);
  }

  async deleteProperty(id: number): Promise<void> {
    await this.api.delete(`/Properties/${id}`);
  }

  // === TENANTS ===
  async getAllTenants(): Promise<TenantDTO[]> {
    const { data } = await this.api.get<TenantDTO[]>('/Tenants');
    return data;
  }

  async getTenantById(id: number): Promise<TenantDTO> {
    const { data } = await this.api.get<TenantDTO>(`/Tenants/${id}`);
    return data;
  }

  async createTenant(tenant: Partial<TenantDTO>): Promise<TenantDTO> {
    const { data } = await this.api.post<TenantDTO>('/Tenants', tenant);
    return data;
  }

  async updateTenant(id: number, tenant: Partial<TenantDTO>): Promise<void> {
    await this.api.put(`/Tenants/${id}`, tenant);
  }

  async deleteTenant(id: number): Promise<void> {
    await this.api.delete(`/Tenants/${id}`);
  }

  // === CONTRACTS ===
  async getAllContracts(): Promise<ContractDTO[]> {
    const { data } = await this.api.get<ContractDTO[]>('/Contracts');
    return data;
  }

  async getContractById(id: number): Promise<ContractDTO> {
    const { data } = await this.api.get<ContractDTO>(`/Contracts/${id}`);
    return data;
  }

  async createContract(contract: Partial<ContractDTO>): Promise<ContractDTO> {
    const { data } = await this.api.post<ContractDTO>('/Contracts', contract);
    return data;
  }

  async updateContract(id: number, contract: Partial<ContractDTO>): Promise<void> {
    await this.api.put(`/Contracts/${id}`, contract);
  }

  async deleteContract(id: number): Promise<void> {
    await this.api.delete(`/Contracts/${id}`);
  }

  // === BILLINGS ===
  async getAllBillings(): Promise<BillingDTO[]> {
    const { data } = await this.api.get<BillingDTO[]>('/Billings');
    return data;
  }

  async getBillingById(id: number): Promise<BillingDTO> {
    const { data } = await this.api.get<BillingDTO>(`/Billings/${id}`);
    return data;
  }

  async createBilling(billing: Partial<BillingDTO>): Promise<BillingDTO> {
    const { data } = await this.api.post<BillingDTO>('/Billings', billing);
    return data;
  }

  async updateBilling(id: number, billing: Partial<BillingDTO>): Promise<void> {
    await this.api.put(`/Billings/${id}`, billing);
  }

  async deleteBilling(id: number): Promise<void> {
    await this.api.delete(`/Billings/${id}`);
  }

  // === PAYMENTS ===
  async getAllPayments(): Promise<PaymentDTO[]> {
    const { data } = await this.api.get<PaymentDTO[]>('/Payments');
    return data;
  }

  async getPaymentById(id: number): Promise<PaymentDTO> {
    const { data } = await this.api.get<PaymentDTO>(`/Payments/${id}`);
    return data;
  }

  async createPayment(payment: Partial<PaymentDTO>): Promise<PaymentDTO> {
    const { data } = await this.api.post<PaymentDTO>('/Payments', payment);
    return data;
  }

  async updatePayment(id: number, payment: Partial<PaymentDTO>): Promise<void> {
    await this.api.put(`/Payments/${id}`, payment);
  }

  async deletePayment(id: number): Promise<void> {
    await this.api.delete(`/Payments/${id}`);
  }

  // === MAINTENANCE REQUESTS ===
  async getAllMaintenanceRequests(): Promise<MaintenanceRequestDTO[]> {
    const { data } = await this.api.get<MaintenanceRequestDTO[]>('/MaintenanceRequests');
    return data;
  }

  async getMaintenanceRequestById(id: number): Promise<MaintenanceRequestDTO> {
    const { data } = await this.api.get<MaintenanceRequestDTO>(`/MaintenanceRequests/${id}`);
    return data;
  }

  async createMaintenanceRequest(request: Partial<MaintenanceRequestDTO>): Promise<MaintenanceRequestDTO> {
    const { data } = await this.api.post<MaintenanceRequestDTO>('/MaintenanceRequests', request);
    return data;
  }

  async updateMaintenanceRequest(id: number, request: Partial<MaintenanceRequestDTO>): Promise<void> {
    await this.api.put(`/MaintenanceRequests/${id}`, request);
  }

  async deleteMaintenanceRequest(id: number): Promise<void> {
    await this.api.delete(`/MaintenanceRequests/${id}`);
  }

  // === DASHBOARD ===
  async getDashboard(): Promise<DashboardDTO> {
    const { data } = await this.api.get<DashboardDTO>('/Dashboard');
    return data;
  }

  // === REPORTS ===
  async getReports(): Promise<any> {
    const { data } = await this.api.get('/Reports');
    return data;
  }

  // === AUDIT LOGS ===
  async getAuditLogs(entity?: string, userName?: string, startDate?: string, endDate?: string): Promise<any> {
    const { data } = await this.api.get('/AuditLogs', {
      params: { entity, userName, startDate, endDate }
    });
    return data;
  }
}

export default new ApiService();
