// Renders a PDF blob inline via an <iframe> (browsers have a built-in PDF viewer).
export default function ResumeViewerModal({ url, name, onClose }) {
    if (!url) return null

    return (
        <div
            onClick={onClose}
            className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4"
        >
            <div
                onClick={(e) => e.stopPropagation()}
                className="flex h-[85vh] w-full max-w-4xl flex-col overflow-hidden rounded-xl bg-white dark:bg-gray-800"
            >
                <div className="flex items-center justify-between border-b border-gray-200 px-4 py-2 dark:border-gray-700">
                    <span className="truncate text-sm font-medium text-gray-700 dark:text-gray-300">{name}</span>
                    <button
                        onClick={onClose}
                        className="rounded-md px-2 py-1 text-sm font-medium text-gray-500 hover:bg-gray-100 dark:text-gray-400 dark:hover:bg-gray-700"
                    >
                        Close
                    </button>
                </div>
                <iframe src={url} title={name} className="flex-1" />
            </div>
        </div>
    )
}
