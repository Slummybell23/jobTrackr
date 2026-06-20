// Example reusable UI component. Build out Modal, Input, etc. alongside it.
export default function Button({ children, className = '', ...props }) {
  return (
    <button
      className={`rounded-md bg-violet-600 px-4 py-2 text-sm font-medium text-white hover:bg-violet-700 disabled:opacity-50 ${className}`}
      {...props}
    >
      {children}
    </button>
  )
}
