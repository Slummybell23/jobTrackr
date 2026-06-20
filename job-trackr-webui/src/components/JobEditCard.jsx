import { useState, useEffect } from 'react'
import client from '../api/client.js'
import Modal from './ui/Modal.jsx'

const STATUSES = ['Bookmarked', 'Applied', 'Phone screen', 'Interviewing', 'Offer', 'Rejected']
const WORK_MODES = ['Remote', 'Hybrid', 'On-site']
const inputClass ="w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-violet-500 focus:outline-none focus:ring-1 focus:ring-violet-500 dark:border-gray-600 dark:bg-gray-900 dark:text-gray-100 dark:placeholder-gray-500"

export default function JobEditCard({ job, isOpen, onClose, onSaved }) {
    const [company, setCompany] = useState('')
    const [role, setRole] = useState('')
    const [link, setLink] = useState('')
    const [location, setLocation] = useState('')
    const [appliedDate, setAppliedDate] = useState('')
    const [status, setStatus] = useState('Applied')
    const [salary, setSalary] = useState('')
    const [workMode, setWorkMode] = useState('')
    const [source, setSource] = useState('')
    const [tags, setTags] = useState('')
    const [description, setDescription] = useState('')
    const [notes, setNotes] = useState('')
    const [error, setError] = useState('')

    // Each time the modal opens (or the job changes), prefill the form with current values.
    useEffect(() => {
        if (!isOpen || !job) return
        setCompany(job.jobCompanyName ?? '')
        setRole(job.jobName ?? '')
        setLink(job.jobLink ?? '')
        setLocation(job.jobLocation ?? '')
        setAppliedDate(job.jobAppliedDate ?? '')
        setStatus(job.jobApplicationStatus ?? 'Applied')
        setSalary(job.jobSalary ?? '')
        setWorkMode(job.jobWorkMode ?? '')
        setSource(job.jobSource ?? '')
        setTags((job.jobTags ?? []).join(', '))
        setDescription(job.jobDescription ?? '')
        setNotes(job.jobNotes ?? '')
        setError('')
    }, [isOpen, job])

    async function handleSubmit(e) {
        e.preventDefault()
        setError('')
        try {
            await client.put('/Job/' + job.jobId, {
                jobCompanyName: company,
                jobName: role,
                jobLink: link,
                jobLocation: location,
                jobAppliedDate: appliedDate || null,
                jobApplicationStatus: status,
                jobDescription: description,
                jobNotes: notes,
                jobSalary: salary,
                jobWorkMode: workMode || null,
                jobSource: source,
                jobTags: tags.split(',').map((t) => t.trim()).filter(Boolean),
                // PUT replaces the whole row — carry these over so they aren't wiped.
                resumeId: job.resumeId,
                jobContactId: job.jobContactId,
            })
            onSaved()   // parent re-fetches the job
            onClose()
        } catch {
            setError('Could not save changes.')
        }
    }

    return (
        <Modal isOpen={isOpen} onClose={onClose}>
            <h3 className="mb-4 text-lg font-semibold text-gray-900 dark:text-gray-100">Edit application</h3>

            <form onSubmit={handleSubmit} className="space-y-3">
                {error && (
                    <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">{error}</div>
                )}

                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Company</label>
                    <input value={company} onChange={(e) => setCompany(e.target.value)} required className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Role</label>
                    <input value={role} onChange={(e) => setRole(e.target.value)} required className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Job URL</label>
                    <input value={link} onChange={(e) => setLink(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Location</label>
                    <input value={location} onChange={(e) => setLocation(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Applied date</label>
                    <input type="date" value={appliedDate} onChange={(e) => setAppliedDate(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Status</label>
                    <select value={status} onChange={(e) => setStatus(e.target.value)} className={inputClass}>
                        {STATUSES.map((s) => (
                            <option key={s} value={s}>{s}</option>
                        ))}
                    </select>
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Work mode</label>
                    <select value={workMode} onChange={(e) => setWorkMode(e.target.value)} className={inputClass}>
                        <option value="">—</option>
                        {WORK_MODES.map((m) => (
                            <option key={m} value={m}>{m}</option>
                        ))}
                    </select>
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Salary</label>
                    <input value={salary} onChange={(e) => setSalary(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Source</label>
                    <input value={source} onChange={(e) => setSource(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Tags (comma-separated)</label>
                    <input value={tags} onChange={(e) => setTags(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Description</label>
                    <textarea rows={3} value={description} onChange={(e) => setDescription(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Notes</label>
                    <textarea rows={3} value={notes} onChange={(e) => setNotes(e.target.value)} className={inputClass} />
                </div>

                <div className="flex justify-end gap-2 pt-2">
                    <button type="button" onClick={onClose} className="rounded-md border border-gray-300 px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-700">
                        Cancel
                    </button>
                    <button type="submit" className="rounded-md bg-violet-600 px-4 py-2 text-sm font-medium text-white hover:bg-violet-700">
                        Save changes
                    </button>
                </div>
            </form>
        </Modal>
    )
}
