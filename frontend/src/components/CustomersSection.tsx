import { useState, useEffect, FormEvent } from 'react';
import { api } from '../api/client';
import type { Customer, CreateCustomerDto } from '../types/models';

export default function CustomersSection() {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Form State
  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState('');
  const [submitting, setSubmitting] = useState(false);

  const loadCustomers = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await api.customers.getCustomers(true);
      setCustomers(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to fetch customers');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCustomers();
  }, []);

  const handleCreateCustomer = async (e: FormEvent) => {
    e.preventDefault();
    if (!fullName || !email) return;

    try {
      setSubmitting(true);
      const newCustomer: CreateCustomerDto = { fullName, email };
      await api.customers.createCustomer(newCustomer);
      
      // Reset form & refresh list
      setFullName('');
      setEmail('');
      await loadCustomers();
    } catch (err) {
      alert(err instanceof Error ? err.message : 'Failed to create customer');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div>
      <h2>Customer Management</h2>

      {/* Create Customer Form */}
      <form onSubmit={handleCreateCustomer} style={{ marginBottom: '30px', padding: '15px', border: '1px solid #ccc' }}>
        <h3>Create New Customer</h3>
        <div style={{ marginBottom: '10px' }}>
          <label style={{ display: 'block' }}>Full Name:</label>
          <input
            type="text"
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
            required
            style={{ width: '100%', padding: '8px' }}
          />
        </div>
        <div style={{ marginBottom: '10px' }}>
          <label style={{ display: 'block' }}>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            style={{ width: '100%', padding: '8px' }}
          />
        </div>
        <button type="submit" disabled={submitting}>
          {submitting ? 'Creating...' : 'Create Customer'}
        </button>
      </form>

      {/* Customers List */}
      <h3>Existing Customers</h3>
      {loading && <p>Loading customers...</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {!loading && !error && (
        <table style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr style={{ textAlign: 'left', borderBottom: '2px solid #ccc' }}>
              <th>ID</th>
              <th>Name</th>
              <th>Email</th>
              <th>Policies Count</th>
            </tr>
          </thead>
          <tbody>
            {customers.map((c) => (
              <tr key={c.customerId} style={{ borderBottom: '1px solid #eee' }}>
                <td><code>{c.customerId.slice(0, 8)}...</code></td>
                <td>{c.fullName}</td>
                <td>{c.email}</td>
                <td>{c.policies?.length ?? 0}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}