import { Outlet } from 'react-router-dom'
import { useState } from 'react'
import Sidebar from './Sidebar.jsx'

// Shell for all authenticated pages: a static sidebar on desktop, a slide-in drawer on mobile.
export default function MainLayout() {
  const [mobileOpen, setMobileOpen] = useState(false)

  return (
    <div className="flex h-screen bg-gray-50 text-gray-900 dark:bg-gray-900 dark:text-gray-100">
      {/* Desktop: persistent sidebar */}
      <div className="hidden lg:block">
        <Sidebar />
      </div>

      {/* Mobile: off-canvas drawer */}
      {mobileOpen && (
        <div className="fixed inset-0 z-40 lg:hidden">
          <div className="absolute inset-0 bg-black/40" onClick={() => setMobileOpen(false)} />
          <div className="absolute left-0 top-0 h-full">
            <Sidebar onNavigate={() => setMobileOpen(false)} />
          </div>
        </div>
      )}

      <div className="flex min-w-0 flex-1 flex-col">
        {/* Mobile top bar with hamburger */}
        <div className="flex items-center gap-3 border-b border-gray-200 bg-white p-3 lg:hidden dark:border-gray-700 dark:bg-gray-800">
          <button
            type="button"
            onClick={() => setMobileOpen(true)}
            aria-label="Open menu"
            className="rounded-md px-2 py-1 text-gray-600 hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-gray-700"
          >
            <span className="text-xl leading-none">☰</span>
          </button>
          <span className="text-lg font-bold text-violet-700 dark:text-violet-400">JobTrackr</span>
        </div>

        <main className="flex-1 overflow-auto p-4 sm:p-6">
          <Outlet />
        </main>
      </div>
    </div>
  )
}
