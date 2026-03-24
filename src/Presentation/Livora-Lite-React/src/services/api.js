import axios from 'axios';
const API_BASE_URL = 'http://localhost:5145/api';
class ApiService {
    constructor() {
        Object.defineProperty(this, "api", {
            enumerable: true,
            configurable: true,
            writable: true,
            value: void 0
        });
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
        this.api.interceptors.response.use((response) => response, (error) => {
            if (error.response?.status === 401) {
                localStorage.removeItem('authToken');
                localStorage.removeItem('user');
                window.location.href = '/login';
            }
            return Promise.reject(error);
        });
    }
    // === AUTH ===
    async login(request) {
        try {
            const { data } = await this.api.post('/Auth/login', request);
            if (data.token) {
                localStorage.setItem('authToken', data.token);
                if (data.user) {
                    localStorage.setItem('user', JSON.stringify(data.user));
                }
            }
            return data;
        }
        catch (error) {
            throw error;
        }
    }
    async register(request) {
        try {
            const { data } = await this.api.post('/Auth/register', request);
            if (data.token) {
                localStorage.setItem('authToken', data.token);
                if (data.user) {
                    localStorage.setItem('user', JSON.stringify(data.user));
                }
            }
            return data;
        }
        catch (error) {
            throw error;
        }
    }
    // === USERS ===
    async getAllUsers() {
        const { data } = await this.api.get('/Users');
        return data;
    }
    async getUserById(id) {
        const { data } = await this.api.get(`/Users/${id}`);
        return data;
    }
    async updateUser(id, user) {
        await this.api.put(`/Users/${id}`, user);
    }
    async deleteUser(id) {
        await this.api.delete(`/Users/${id}`);
    }
    // === PROPERTIES ===
    async getAllProperties() {
        const { data } = await this.api.get('/Properties');
        return data;
    }
    async getPropertyById(id) {
        const { data } = await this.api.get(`/Properties/${id}`);
        return data;
    }
    async createProperty(property) {
        const { data } = await this.api.post('/Properties', property);
        return data;
    }
    async updateProperty(id, property) {
        await this.api.put(`/Properties/${id}`, property);
    }
    async deleteProperty(id) {
        await this.api.delete(`/Properties/${id}`);
    }
    // === TENANTS ===
    async getAllTenants() {
        const { data } = await this.api.get('/Tenants');
        return data;
    }
    async getTenantById(id) {
        const { data } = await this.api.get(`/Tenants/${id}`);
        return data;
    }
    async createTenant(tenant) {
        const { data } = await this.api.post('/Tenants', tenant);
        return data;
    }
    async updateTenant(id, tenant) {
        await this.api.put(`/Tenants/${id}`, tenant);
    }
    async deleteTenant(id) {
        await this.api.delete(`/Tenants/${id}`);
    }
    // === CONTRACTS ===
    async getAllContracts() {
        const { data } = await this.api.get('/Contracts');
        return data;
    }
    async getContractById(id) {
        const { data } = await this.api.get(`/Contracts/${id}`);
        return data;
    }
    async createContract(contract) {
        const { data } = await this.api.post('/Contracts', contract);
        return data;
    }
    async updateContract(id, contract) {
        await this.api.put(`/Contracts/${id}`, contract);
    }
    async deleteContract(id) {
        await this.api.delete(`/Contracts/${id}`);
    }
    // === BILLINGS ===
    async getAllBillings() {
        const { data } = await this.api.get('/Billings');
        return data;
    }
    async getBillingById(id) {
        const { data } = await this.api.get(`/Billings/${id}`);
        return data;
    }
    async createBilling(billing) {
        const { data } = await this.api.post('/Billings', billing);
        return data;
    }
    async updateBilling(id, billing) {
        await this.api.put(`/Billings/${id}`, billing);
    }
    async deleteBilling(id) {
        await this.api.delete(`/Billings/${id}`);
    }
    // === PAYMENTS ===
    async getAllPayments() {
        const { data } = await this.api.get('/Payments');
        return data;
    }
    async getPaymentById(id) {
        const { data } = await this.api.get(`/Payments/${id}`);
        return data;
    }
    async createPayment(payment) {
        const { data } = await this.api.post('/Payments', payment);
        return data;
    }
    async updatePayment(id, payment) {
        await this.api.put(`/Payments/${id}`, payment);
    }
    async deletePayment(id) {
        await this.api.delete(`/Payments/${id}`);
    }
    // === MAINTENANCE REQUESTS ===
    async getAllMaintenanceRequests() {
        const { data } = await this.api.get('/MaintenanceRequests');
        return data;
    }
    async getMaintenanceRequestById(id) {
        const { data } = await this.api.get(`/MaintenanceRequests/${id}`);
        return data;
    }
    async createMaintenanceRequest(request) {
        const { data } = await this.api.post('/MaintenanceRequests', request);
        return data;
    }
    async updateMaintenanceRequest(id, request) {
        await this.api.put(`/MaintenanceRequests/${id}`, request);
    }
    async deleteMaintenanceRequest(id) {
        await this.api.delete(`/MaintenanceRequests/${id}`);
    }
    // === DASHBOARD ===
    async getDashboard() {
        const { data } = await this.api.get('/Dashboard');
        return data;
    }
    // === REPORTS ===
    async getReports() {
        const { data } = await this.api.get('/Reports');
        return data;
    }
    // === AUDIT LOGS ===
    async getAuditLogs(entity, userName, startDate, endDate) {
        const { data } = await this.api.get('/AuditLogs', {
            params: { entity, userName, startDate, endDate }
        });
        return data;
    }
}
export default new ApiService();
