//enum emulation 
export const PolicyTypeMap = {
  0: 'Auto',
  1: 'Property',
  2: 'Health',
} as const;
//in case unexpected policy type comes back
export const getPolicyTypeName = (typeId: number): string => {
  return typeId in PolicyTypeMap
    ? PolicyTypeMap[typeId as keyof typeof PolicyTypeMap]
    : `Unknown Type (${typeId})`;
};

export interface Policy {
  policyId: string;
  policyNumber: number;
  policyType: number;
  coverageAmount: number;
  validFrom: string;
  validTo: string;
}

export interface Customer {
  customerId: string;
  fullName: string;
  email: string;
  createdAt: string;
  policies?: Policy[];
}

export interface ClaimDto {
  claimId: string;
  policyId: string;
  amount: number;
  description: string;
  incidentDate: string;
  status: string; 
  createdAt: string;
  decidedAt?: string;
  decisionReason?: string;
}

export interface ClaimHistoryDto {
  claimStatusHistoryId: string;
  claimId: string;
  fromStatus: string;
  toStatus: string;
  changedAt: string;
  changedBy: string;
  comment: string;
}

export interface CreateCustomerDto {
  fullName: string;
  email: string;
}

export interface CreatePolicyDto {
  customerId: string;
  policyNumber: number;
  policyType: number;
  coverageAmount: number;
  validFrom: string;
  validTo: string;
}

export interface PolicyResponse {
  policyId: string;
  policyNumber: number;
  policyType: number;
  coverageAmount: number;
  validFrom: string;
  validTo: string;
  customer: Customer;
  claims?: ClaimDto[];
  claimStatusHistories?: ClaimHistoryDto[];
}

export interface CreateClaimDto{
  policyId: string;
  amount: number;
  description: string;
  incidentDate: string;
}