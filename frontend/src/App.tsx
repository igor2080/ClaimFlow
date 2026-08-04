import { useState } from 'react';
import CustomersSection from './components/CustomersSection';
import PoliciesSection from './components/PoliciesSection';
import ClaimsSection from './components/ClaimsSection';

type Tab = 'customers' | 'policies' | 'claims';

export default function App() {
  const [activeTab, setActiveTab] = useState<Tab>('customers');

  return (
    <div style={{ maxWidth: '1000px', margin: '0 auto', padding: '20px', fontFamily: 'sans-serif' }}>
      <h1>ClaimFlow Dashboard</h1>

      {/* Navigation Tabs */}
      <nav style={{ display: 'flex', gap: '10px', marginBottom: '20px', borderBottom: '2px solid #ddd' }}>
        <button
          style={{ padding: '10px 20px', fontWeight: activeTab === 'customers' ? 'bold' : 'normal' }}
          onClick={() => setActiveTab('customers')}
        >
          Customers
        </button>
        <button
          style={{ padding: '10px 20px', fontWeight: activeTab === 'policies' ? 'bold' : 'normal' }}
          onClick={() => setActiveTab('policies')}
        >
          Policies
        </button>
        <button
          style={{ padding: '10px 20px', fontWeight: activeTab === 'claims' ? 'bold' : 'normal' }}
          onClick={() => setActiveTab('claims')}
        >
          Claims
        </button>
      </nav>

      {/* Tab Panels */}
      <main>
        {activeTab === 'customers' && <CustomersSection />}
        {activeTab === 'policies' && <PoliciesSection />}
        {activeTab === 'claims' && <ClaimsSection />}
      </main>
    </div>
  );
}