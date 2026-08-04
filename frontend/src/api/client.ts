import type * as models from '../types/models';

const API_BASE_URL = import.meta.env.VITE_API_URL || '';

interface FetchOptions extends Omit<RequestInit, 'body'> {
    body?: unknown;
}

export interface ApiResponse {
    id: string;
    message: string;
}

async function fetchJson<T>(endpoint: string, options: FetchOptions = {}): Promise<T> {
    const { body, headers, method, ...customConfig } = options;

    const config: RequestInit = {
        method: method ?? (body ? 'POST' : 'GET'), //defaulting fallback to POST if there's a body
        headers: {
            'Content-Type': 'application/json',
            ...headers,
        },
        ...customConfig,
    };

    if (body) {
        config.body = JSON.stringify(body);
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, config);

    if (!response.ok) {
        const errorText = await response.text().catch(() => '');
        throw new Error(
            `API Error (${response.status}): ${errorText || response.statusText}`
        );
    }

    return response.json();
}

export const api = {
    customers: {
        createCustomer: (customer: models.CreateCustomerDto): Promise<ApiResponse> => {
            return fetchJson<ApiResponse>(`/api/CreateCustomer`, {
                method: 'POST',
                body: customer,
            });
        },
        getCustomerById: (id: string): Promise<models.Customer> => {
            return fetchJson<models.Customer>(`/api/GetCustomer/${id}`, {
                method: 'GET',
            });
        },
        getCustomers: (withPolicies = true): Promise<models.Customer[]> => {
            return fetchJson<models.Customer[]>(`/api/GetCustomers?withPolicies=${withPolicies}`);
        },
    },
    policies: {
        createPolicy: (policy: models.CreatePolicyDto): Promise<ApiResponse> => {
            return fetchJson<ApiResponse>(`/api/CreatePolicy`, {
                method: 'POST',
                body: policy
            });
        },
        getPolicyById: (id: string, withHistory = true): Promise<models.PolicyResponse> => {
            return fetchJson<models.PolicyResponse>(`/api/GetPolicy/${id}?withHistory=${withHistory}`, {
                method: 'GET',
            });
        },
    },
    claims: {
        createClaim: (claim: models.CreateClaimDto): Promise<ApiResponse> => {
            return fetchJson<ApiResponse>(`/api/CreateClaim`, {
                method: 'POST',
                body: claim,
            });
        },
    },
};