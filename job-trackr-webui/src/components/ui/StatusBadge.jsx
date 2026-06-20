export const STATUS_STYLES = {
    'Bookmarked':   { dot: 'bg-gray-400',   badge: 'bg-gray-100 text-gray-700' },
    'Applied':      { dot: 'bg-blue-500',   badge: 'bg-blue-100 text-blue-700' },
    'Phone screen': { dot: 'bg-purple-500', badge: 'bg-purple-100 text-purple-700' },
    'Interviewing': { dot: 'bg-amber-500',  badge: 'bg-amber-100 text-amber-700' },
    'Offer':        { dot: 'bg-green-500',  badge: 'bg-green-100 text-green-700' },
    'Rejected':     { dot: 'bg-red-500',    badge: 'bg-red-100 text-red-700' },
}

const DEFAULT_STYLE = { dot: 'bg-gray-400', badge: 'bg-gray-100 text-gray-700' }

// Falls back to a neutral gray for any status not in the map.
export function statusStyle(status) {
    return STATUS_STYLES[status] ?? DEFAULT_STYLE
}

export default function StatusBadge({ status }) {
    return (
        <span className={`inline-flex items-center rounded-full px-2 py-0.5 text-xs font-medium ${statusStyle(status).badge}`}>
            {status}
        </span>
    )
}
