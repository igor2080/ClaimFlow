import type { SubmitEvent } from 'react';
import { useState, useEffect } from 'react';
import { api } from '../api/client';
import {
  getPolicyTypeName,
  PolicyTypeMap,
  type Customer,
  type CreatePolicyDto,
  type PolicyResponse,
} from '../types/models';

export default function PoliciesSection() {
  // Data State
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [policyDetail, setPolicyDetail] = useState<PolicyResponse | null>(null);

  // Form State - Create Policy
  const [selectedCustomerId, setSelectedCustomerId] = useState('');
  const [policyNumber, setPolicyNumber] = useState<number>(10001);
  const [policyType, setPolicyType] = useState<number>(0); // Default: Auto (0)
  const [coverageAmount, setCoverageAmount] = useState<number>(50000);
  const [validFrom, setValidFrom] = useState(new Date().toISOString().split('T')[0]);
  const [validTo, setValidTo] = useState(
    new Date(Date.now() + 365 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]
  );

  // Search State
  const [searchId, setSearchId] = useState('');
  const [withHistory, setWithHistory] = useState(true);

  // Status State
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Load customers on mount for the dropdown selection
  useEffect(() => {
    api.customers
      .getCustomers(false)
      .then((data) => {
        setCustomers(data);
        if (data.length > 0) setSelectedCustomerId(data[0].customerId);
      })
      .catch((err) => setError(`Failed to load customers for dropdown selection. (${err})`));
  }, []);

  // Handle Policy Creation
  const handleCreatePolicy = async (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!selectedCustomerId) return;

    try {
      setSubmitting(true);
      setError(null);

      const newPolicy: CreatePolicyDto = {
        customerId: selectedCustomerId,
        policyNumber: Number(policyNumber),
        policyType: Number(policyType),
        coverageAmount: Number(coverageAmount),
        validFrom: new Date(validFrom).toISOString(),
        validTo: new Date(validTo).toISOString(),
      };

      const res = await api.policies.createPolicy(newPolicy);
      alert(`Success: ${res.message}`);

      // Auto-load the newly created policy
      setSearchId(res.id);
      await fetchPolicyDetails(res.id, withHistory);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create policy');
    } finally {
      setSubmitting(false);
    }
  };

  // Handle Lookup by ID
  const fetchPolicyDetails = async (id: string, includeHistory: boolean) => {
    if (!id.trim()) return;
    try {
      setLoading(true);
      setError(null);
      const data = await api.policies.getPolicyById(id, includeHistory);
      setPolicyDetail(data);
    } catch (err) {
      setPolicyDetail(null);
      setError(err instanceof Error ? err.message : 'Policy not found');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Policy Management</h2>

      {error && <p style={{ color: 'red', fontWeight: 'bold' }}>{error}</p>}

      {/* Grid Layout: Create Form + Lookup Panel */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px', marginBottom: '30px' }}>
        
        {/* Create Policy Form */}
        <form onSubmit={handleCreatePolicy} style={{ padding: '15px', border: '1px solid #ccc' }}>
          <h3>Create Policy</h3>

          <div style={{ marginBottom: '10px' }}>
            <label style={{ display: 'block' }}>Customer:</label>
            <select
              value={selectedCustomerId}
              onChange={(e) => setSelectedCustomerId(e.target.value)}
              required
              style={{ width: '100%', padding: '8px' }}
            >
              <option value="" disabled>Select Customer</option>
              {customers.map((c) => (
                <option key={c.customerId} value={c.customerId}>
                  {c.fullName} ({c.email})
                </option>
              ))}
            </select>
          </div>

          <div style={{ marginBottom: '10px' }}>
            <label style={{ display: 'block' }}>Policy Number:</label>
            <input
              type="number"
              value={policyNumber}
              onChange={(e) => setPolicyNumber(Number(e.target.value))}
              required
              style={{ width: '100%', padding: '8px' }}
            />
          </div>

          <div style={{ marginBottom: '10px' }}>
            <label style={{ display: 'block' }}>Policy Type:</label>
            <select
              value={policyType}
              onChange={(e) => setPolicyType(Number(e.target.value))}
              style={{ width: '100%', padding: '8px' }}
            >
              {Object.entries(PolicyTypeMap).map(([key, label]) => (
                <option key={key} value={key}>
                  {label} ({key})
                </option>
              ))}
            </select>
          </div>

          <div style={{ marginBottom: '10px' }}>
            <label style={{ display: 'block' }}>Coverage Amount ($):</label>
            <input
              type="number"
              value={coverageAmount}
              onChange={(e) => setCoverageAmount(Number(e.target.value))}
              required
              style={{ width: '100%', padding: '8px' }}
            />
          </div>

          <div style={{ marginBottom: '10px' }}>
            <label style={{ display: 'block' }}>Valid From:</label>
            <input
              type="date"
              value={validFrom}
              onChange={(e) => setValidFrom(e.target.value)}
              required
              style={{ width: '100%', padding: '8px' }}
            />
          </div>

          <div style={{ marginBottom: '10px' }}>
            <label style={{ display: 'block' }}>Valid To:</label>
            <input
              type="date"
              value={validTo}
              onChange={(e) => setValidTo(e.target.value)}
              required
              style={{ width: '100%', padding: '8px' }}
            />
          </div>

          <button type="submit" disabled={submitting}>
            {submitting ? 'Creating...' : 'Create Policy'}
          </button>
        </form>

        {/* Lookup Policy Form */}
        <div style={{ padding: '15px', border: '1px solid #ccc' }}>
          <h3>Lookup Policy by ID</h3>

          <div style={{ marginBottom: '10px' }}>
            <input
              type="text"
              placeholder="Enter Policy GUID..."
              value={searchId}
              onChange={(e) => setSearchId(e.target.value)}
              style={{ width: '100%', padding: '8px', marginBottom: '10px' }}
            />
            <label style={{ display: 'inline-flex', alignItems: 'center', gap: '5px' }}>
              <input
                type="checkbox"
                checked={withHistory}
                onChange={(e) => setWithHistory(e.target.checked)}
              />
              Include Status History
            </label>
          </div>

          <button
            onClick={() => fetchPolicyDetails(searchId, withHistory)}
            disabled={loading || !searchId.trim()}
          >
            {loading ? 'Fetching...' : 'Get Policy Details'}
          </button>
        </div>
      </div>

      {/* Policy Details Results Card */}
      {policyDetail && (
        <div style={{ padding: '20px', border: '2px solid #007bb5', borderRadius: '5px' }}>
          <h3>Policy Details (#{policyDetail.policyNumber})</h3>
          <p><strong>ID:</strong> {policyDetail.policyId}</p>
          <p><strong>Type:</strong> {getPolicyTypeName(policyDetail.policyType)}</p>
          <p><strong>Coverage:</strong> ${policyDetail.coverageAmount.toLocaleString()}</p>
          <p>
            <strong>Valid Period:</strong>{' '}
            {new Date(policyDetail.validFrom).toLocaleDateString()} to{' '}
            {new Date(policyDetail.validTo).toLocaleDateString()}
          </p>

          <hr />
          <h4>Customer</h4>
          <p><strong>Name:</strong> {policyDetail.customer.fullName}</p>
          <p><strong>Email:</strong> {policyDetail.customer.email}</p>

          <hr />
          <h4>Claims ({policyDetail.claims?.length ?? 0})</h4>
          {policyDetail.claims && policyDetail.claims.length > 0 ? (
            <ul>
              {policyDetail.claims.map((claim) => (
                <li key={claim.claimId}>
                  <strong>{claim.status}</strong> - ${claim.amount} ({claim.description})
                </li>
              ))}
            </ul>
          ) : (
            <p>No claims filed under this policy.</p>
          )}

          {/* Status Histories Table */}
          {withHistory && policyDetail.claimStatusHistories && (
            <>
              <hr />
              <h4>Claim Status History Log</h4>
              {policyDetail.claimStatusHistories.length > 0 ? (
                <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.9em' }}>
                  <thead>
                    <tr style={{ textAlign: 'left', borderBottom: '1px solid #ccc' }}>
                      <th>Claim ID</th>
                      <th>Transition</th>
                      <th>Changed By</th>
                      <th>Date</th>
                      <th>Comment</th>
                    </tr>
                  </thead>
                  <tbody>
                    {policyDetail.claimStatusHistories.map((h) => (
                      <tr key={h.claimStatusHistoryId} style={{ borderBottom: '1px solid #eee' }}>
                        <td><code>{h.claimId.slice(0, 8)}...</code></td>
                        <td>{h.fromStatus} &rarr; {h.toStatus}</td>
                        <td>{h.changedBy}</td>
                        <td>{new Date(h.changedAt).toLocaleString()}</td>
                        <td>{h.comment || '-'}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              ) : (
                <p>No history records logged yet.</p>
              )}
            </>
          )}
        </div>
      )}
    </div>
  );
}