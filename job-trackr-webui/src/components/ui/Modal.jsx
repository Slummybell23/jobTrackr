export default function Modal({ isOpen, onClose, children }) {
    if (!isOpen) return null   // render nothing when closed

    return (
        <div
            onClick={onClose}
            className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4"
        >
            <div
                onClick={(e) => e.stopPropagation()}
                className="w-full max-w-md rounded-xl bg-white p-6 shadow-lg dark:bg-gray-800"
            >
                {children}
            </div>
        </div>
    )
}