import { useState, useEffect } from 'react'
import './App.css'

//enum emulation 
const PolicyTypeMap = {
  0: 'Auto',
  1: 'Property',
  2: 'Health',
} as const;
//in case unexpected policy type comes back
const getPolicyTypeName = (typeId: number): string => {
  return typeId in PolicyTypeMap 
    ? PolicyTypeMap[typeId as keyof typeof PolicyTypeMap] 
    : `Unknown Type (${typeId})`;
};

interface Policy {
  policyId: string;
  policyNumber: number;
  policyType: number;
  coverageAmount: number;
  validFrom: string;
  validTo: string;
}

interface Customer {
  customerId: string;
  fullName: string;
  email: string;
  createdAt: string;
  policies: Policy[];
}

function App() {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [loading, setLoading] = useState<Boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost';

  useEffect(() => {
    const fetch_customers = async () => {
      try {
        setLoading(true);
        const response = await fetch(`${API_BASE_URL}/api/GetCustomers?withPolicies=true`);

        if (!response.ok) {
          throw new Error(`API Error: ${response.status}`);
        }

        const data: Customer[] = await response.json();
        setCustomers(data);
      } catch (err: any) {
        setError(err.message || 'Error connecting to the API.');
      } finally {
        setLoading(false);
      }
    };

    fetch_customers();
  }, [API_BASE_URL]);
  if (loading) return <div style={{ padding: '20px' }}>Loading...</div>;
  if (error) return <div style={{ padding: '20px', color: 'red' }}>Error: {error}</div>;

  return(
    <div style={{ padding: '40px', fontFamily: 'sans-serif', backgroundColor: '#fafafa', minHeight: '100vh' }}>
      <h1>ClaimFlow Management Dashboard</h1>
      
      {customers.length === 0 ? (
        <p>No customers found.</p>
      ) : (
        customers.map((customer) => (
          <div key={customer.customerId} style={{ 
            background: '#fff', 
            padding: '20px', 
            marginBottom: '20px', 
            borderRadius: '8px', 
            boxShadow: '0 2px 4px rgba(0,0,0,0.05)' 
          }}>
            <h2 style={{ margin: '0 0 5px 0' }}>{customer.fullName}</h2>
            <p style={{ color: '#666', margin: '0 0 15px 0' }}>{customer.email} | <small><code>{customer.customerId}</code></small></p>

            <h3>Active Policies ({customer.policies?.length || 0})</h3>
            {!customer.policies || customer.policies.length === 0 ? (
              <p style={{ color: '#999', fontStyle: 'italic' }}>No policies registered to this profile.</p>
            ) : (
              <table border={1} cellPadding={8} style={{ borderCollapse: 'collapse', width: '100%', borderColor: '#eee' }}>
                <thead>
                  <tr style={{ backgroundColor: '#f9f9f9', textAlign: 'left' }}>
                    <th>Policy #</th>
                    <th>Type</th>
                    <th>Coverage</th>
                    <th>Term Dates</th>
                  </tr>
                </thead>
                <tbody>
                  {customer.policies.map((policy) => (
                    <tr key={policy.policyId}>
                      <td><code>{policy.policyNumber}</code></td>
                      <td>
                        <strong style={{ color: '#2b6cb0' }}>
                          {getPolicyTypeName(policy.policyType)}
                        </strong>
                      </td>
                      <td>${policy.coverageAmount.toLocaleString()}</td>
                      <td>
                        <small>
                          {new Date(policy.validFrom).toLocaleDateString()} to {new Date(policy.validTo).toLocaleDateString()}
                        </small>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        ))
      )}
    </div>
  );
}

export default App
