import { NavLink, useNavigate } from 'react-router-dom'
import { useState } from 'react'
import { useAuth } from '../../context/AuthContext.jsx'

const links = [
  { to: '/', label: '📋 Board', end: true },
  { to: '/dashboard', label: '📊 Dashboard' },
  { to: '/reminders', label: '🔔 Reminders' },
  { to: '/resumes', label: '📄 Resumes' },
  { to: '/contacts', label: '👤 Contacts' },
]

// onNavigate lets the mobile drawer close itself when a link is tapped.
export default function Sidebar({ onNavigate }) {
  const { logout } = useAuth()
  const navigate = useNavigate()
  const [dark, setDark] = useState(() => document.documentElement.classList.contains('dark'))

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const toggleTheme = () => {
    const next = !dark
    setDark(next)
    document.documentElement.classList.toggle('dark', next)
    localStorage.setItem('theme', next ? 'dark' : 'light')
  }

  return (
    <aside className="flex h-full w-56 flex-col border-r border-gray-200 bg-white p-4 dark:border-gray-700 dark:bg-gray-800">
      <h1 className="mb-6 text-xl font-bold text-violet-700 dark:text-violet-400">JobTrackr</h1>

      <nav className="flex flex-col gap-1">
        {links.map((link) => (
          <NavLink
            key={link.to}
            to={link.to}
            end={link.end}
            onClick={onNavigate}
            className={({ isActive }) =>
              `rounded-md px-3 py-2 text-sm font-medium ${
                isActive
                  ? 'bg-violet-100 text-violet-700 dark:bg-violet-500/15 dark:text-violet-300'
                  : 'text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700'
              }`
            }
          >
            {link.label}
          </NavLink>
        ))}
      </nav>

      <button
        type="button"
        onClick={toggleTheme}
        className="mt-auto rounded-md px-3 py-2 text-left text-sm font-medium text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700"
      >
        {dark ? '☀️ Light mode' : '🌙 Dark mode'}
      </button>

      <button
        type="button"
        onClick={handleLogout}
        className="rounded-md px-3 py-2 text-left text-sm font-medium text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700"
      >
        Logout
      </button>
    </aside>
  )
}
