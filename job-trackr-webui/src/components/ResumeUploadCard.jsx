import { useState } from 'react'
import client from '../api/client.js'
import Modal from './ui/Modal.jsx'

const inputClass = "w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-violet-500 focus:outline-none focus:ring-1 focus:ring-violet-500 dark:border-gray-600 dark:bg-gray-900 dark:text-gray-100 dark:placeholder-gray-500"

export default function ResumeUploadCard({ isOpen, onClose, onUploaded }) {
    const [file, setFile] = useState(null)
    const [name, setName] = useState('')
    const [notes, setNotes] = useState('')
    const [labels, setLabels] = useState('')
    const [error, setError] = useState('')

    async function handleSubmit(e) {
        e.preventDefault()
        setError('')
        if (!file) {
            setError('Please choose a file.')
            return
        }

        // Uploads are multipart/form-data, not JSON. Field names match CreateResumeRq.
        const formData = new FormData()
        formData.append('File', file)
        formData.append('ResumeName', name)
        if (notes) formData.append('ResumeNotes', notes)
        labels
            .split(',')
            .map((l) => l.trim())
            .filter(Boolean)
            .forEach((l) => formData.append('ResumeLabels', l))   // repeated field -> List<string>

        try {
            await client.post('/Resume/upload', formData)
            onUploaded()
            setFile(null)
            setName('')
            setNotes('')
            setLabels('')
            onClose()
        } catch {
            setError('Could not upload. Check the file type (.pdf, .doc, .docx) and try again.')
        }
    }

    return (
        <Modal isOpen={isOpen} onClose={onClose}>
            <h3 className="mb-4 text-lg font-semibold text-gray-900 dark:text-gray-100">Upload resume</h3>

            <form onSubmit={handleSubmit} className="space-y-3">
                {error && (
                    <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">{error}</div>
                )}

                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">File</label>
                    <input
                        type="file"
                        accept=".pdf,.doc,.docx"
                        onChange={(e) => setFile(e.target.files[0])}
                        className="block w-full text-sm text-gray-700 dark:text-gray-300"
                    />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Name</label>
                    <input value={name} onChange={(e) => setName(e.target.value)} required className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Notes</label>
                    <input value={notes} onChange={(e) => setNotes(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Labels (comma-separated)</label>
                    <input value={labels} onChange={(e) => setLabels(e.target.value)} placeholder="general, backend, faang" className={inputClass} />
                </div>

                <div className="flex justify-end gap-2 pt-2">
                    <button type="button" onClick={onClose} className="rounded-md border border-gray-300 px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-700">
                        Cancel
                    </button>
                    <button type="submit" className="rounded-md bg-violet-600 px-4 py-2 text-sm font-medium text-white hover:bg-violet-700">
                        Upload
                    </button>
                </div>
            </form>
        </Modal>
    )
}
