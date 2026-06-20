import { useState, useEffect } from 'react'
import client from '../api/client.js'

const selectClass = "w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-violet-500 focus:outline-none focus:ring-1 focus:ring-violet-500 dark:border-gray-600 dark:bg-gray-900 dark:text-gray-100"

// Resume + contact dropdowns that set Job.ResumeId / Job.JobContactId via PUT.
export default function JobLinks({ job, onUpdated }) {
    const [resumes, setResumes] = useState([])
    const [contacts, setContacts] = useState([])
    const [error, setError] = useState('')

    useEffect(() => {
        Promise.all([client.get('/Resume/list'), client.get('/Contact/list')])
            .then(([r, c]) => {
                setResumes(r.data)
                setContacts(c.data)
            })
            .catch(() => setError('Could not load resumes and contacts.'))
    }, [])

    async function updateLink(field, value) {
        setError('')
        try {
            // PUT is a full replace, so send every field and override just the linked id.
            await client.put('/Job/' + job.jobId, {
                jobCompanyName: job.jobCompanyName,
                jobName: job.jobName,
                jobDescription: job.jobDescription,
                jobLink: job.jobLink,
                jobNotes: job.jobNotes,
                jobApplicationStatus: job.jobApplicationStatus,
                jobLocation: job.jobLocation,
                jobAppliedDate: job.jobAppliedDate,
                jobSalary: job.jobSalary,
                jobWorkMode: job.jobWorkMode,
                jobSource: job.jobSource,
                jobTags: job.jobTags,
                resumeId: job.resumeId,
                jobContactId: job.jobContactId,
                [field]: value,
            })
            onUpdated()
        } catch {
            setError('Could not update the link.')
        }
    }

    return (
        <div className="mt-4 rounded-xl border border-gray-200 bg-white p-6 dark:border-gray-700 dark:bg-gray-800">
            <h3 className="mb-3 text-sm font-semibold text-gray-900 dark:text-gray-100">Links</h3>

            {error && (
                <div className="mb-3 rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700 dark:border-red-900 dark:bg-red-950 dark:text-red-300">{error}</div>
            )}

            <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Resume sent</label>
                    <select
                        value={job.resumeId ?? ''}
                        onChange={(e) => updateLink('resumeId', e.target.value ? Number(e.target.value) : null)}
                        className={selectClass}
                    >
                        <option value="">None</option>
                        {resumes.map((r) => (
                            <option key={r.resumeId} value={r.resumeId}>{r.resumeName}</option>
                        ))}
                    </select>
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Contact</label>
                    <select
                        value={job.jobContactId ?? ''}
                        onChange={(e) => updateLink('jobContactId', e.target.value ? Number(e.target.value) : null)}
                        className={selectClass}
                    >
                        <option value="">None</option>
                        {contacts.map((c) => (
                            <option key={c.contactId} value={c.contactId}>{c.contactFN} {c.contactLN}</option>
                        ))}
                    </select>
                </div>
            </div>
        </div>
    )
}
