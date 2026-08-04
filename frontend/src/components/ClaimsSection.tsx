import { useState, FormEvent } from 'react';
import { api } from '../api/client';
import type { CreateClaimDto } from '../types/models';

export default function ClaimsSection() {
  const [policyId, setPolicyId] = useState('');
  const [amount, setAmount] = useState<number>(1500);
  const [description, setDescription] = useState('');
  const [incidentDate, setIncidentDate] = useState(
    new Date().toISOString().split('T')[0]
  );

  const [submitting, setSubmitting] = useState(false);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const handleCreateClaim = async (e: FormEvent) => {
    e.preventDefault();
    if (!policyId.trim() || !description.trim()) return;

    try {
      setSubmitting(true);
      setError(null);
      setSuccessMessage(null);

      const claimData: CreateClaimDto = {
        policyId: policyId.trim(),
        amount: Number(amount),
        description: description.trim(),
        incidentDate: new Date(incidentDate).toISOString(),
      };

      const res = await api.claims.createClaim(claimData);
      setSuccessMessage(`${res.message} (Claim ID: ${res.id})`);

      // Reset form fields
      setDescription('');
      setAmount(1500);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to file claim');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div>
      <h2>Claims Management</h2>

      <form
        onSubmit={handleCreateClaim}
        style={{ maxWidth: '600px', padding: '20px', border: '1px solid #ccc' }}
      >
        <h3>File a New Claim</h3>

        {error && (
          <div style={{ padding: '10px', backgroundColor: '#ffe6e6', color: '#cc0000', marginBottom: '15px' }}>
            {error}
          </div>
        )}

        {successMessage && (
          <div style={{ padding: '10px', backgroundColor: '#e6ffe6', color: '#008000', marginBottom: '15px' }}>
            {successMessage}
          </div>
        )}

        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Policy ID (GUID):</label>
          <input
            type="text"
            value={policyId}
            onChange={(e) => setPolicyId(e.target.value)}
            placeholder="e.g. 3fa85f64-5717-4562-b3fc-2c963f66afa6"
            required
            style={{ width: '100%', padding: '8px' }}
          />
        </div>

        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Claim Amount ($):</label>
          <input
            type="number"
            min="1"
            step="0.01"
            value={amount}
            onChange={(e) => setAmount(Number(e.target.value))}
            required
            style={{ width: '100%', padding: '8px' }}
          />
        </div>

        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Incident Date:</label>
          <input
            type="date"
            value={incidentDate}
            onChange={(e) => setIncidentDate(e.target.value)}
            max={new Date().toISOString().split('T')[0]} // Prevents future dates in datepicker
            required
            style={{ width: '100%', padding: '8px' }}
          />
        </div>

        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', marginBottom: '5px' }}>Incident Description:</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={4}
            placeholder="Describe what happened..."
            required
            style={{ width: '100%', padding: '8px' }}
          />
        </div>

        <button type="submit" disabled={submitting} style={{ padding: '10px 20px' }}>
          {submitting ? 'Submitting Claim...' : 'Submit Claim'}
        </button>
      </form>
    </div>
  );
}