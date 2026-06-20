import { Routes, Route, Navigate, useLocation, useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'
import client from './api/client.js'
import MainLayout from './components/layout/MainLayout.jsx'
import ProtectedRoute from './components/ProtectedRoute.jsx'
import SetupPage from './pages/SetupPage.jsx'
import LoginPage from './pages/LoginPage.jsx'
import RegisterPage from './pages/RegisterPage.jsx'
import KanbanPage from './pages/KanbanPage.jsx'
import DashboardPage from './pages/DashboardPage.jsx'
import ApplicationDetailPage from './pages/ApplicationDetailPage.jsx'
import RemindersPage from './pages/RemindersPage.jsx'
import ResumesPage from './pages/ResumesPage.jsx'
import ContactsPage from './pages/ContactsPage.jsx'

// Until the app reports it's configured, force everything to the first-run /setup wizard.
function SetupGate({ children }) {
  const [status, setStatus] = useState('loading')
  const location = useLocation()
  const navigate = useNavigate()

  useEffect(() => {
    client.get('/setup/status')
      .then((r) => setStatus(r.data.isConfigured ? 'configured' : 'needsSetup'))
      .catch(() => setStatus('needsSetup'))
  }, [])

  useEffect(() => {
    if (status === 'needsSetup' && location.pathname !== '/setup') {
      navigate('/setup', { replace: true })
    } else if (status === 'configured' && location.pathname === '/setup') {
      navigate('/login', { replace: true })
    }
  }, [status, location.pathname, navigate])

  if (status === 'loading') return null
  return children
}

function App() {
  return (
    <SetupGate>
      <Routes>
        {/* First-run wizard */}
        <Route path="/setup" element={<SetupPage />} />

        {/* Public routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        {/* Authenticated routes: redirect to /login if no token, then render inside the sidebar layout */}
        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/" element={<KanbanPage />} />
            <Route path="/dashboard" element={<DashboardPage />} />
            <Route path="/applications/:id" element={<ApplicationDetailPage />} />
            <Route path="/reminders" element={<RemindersPage />} />
            <Route path="/resumes" element={<ResumesPage />} />
            <Route path="/contacts" element={<ContactsPage />} />
          </Route>
        </Route>

        {/* Anything else goes home */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </SetupGate>
  )
}

export default App
