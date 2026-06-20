import { useState, useEffect } from 'react'
import client from '../api/client.js'

const inputClass = "w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-violet-500 focus:outline-none focus:ring-1 focus:ring-violet-500 dark:border-gray-600 dark:bg-gray-900 dark:text-gray-100 dark:placeholder-gray-500"

// Lists this job's reminders and lets you add / complete / delete them.
export default function JobReminders({ jobId }) {
    const [reminders, setReminders] = useState([])
    const [title, setTitle] = useState('')
    const [notes, setNotes] = useState('')
    const [date, setDate] = useState(() => new Date().toISOString().slice(0, 10))
    const [error, setError] = useState('')

    async function pull() {
        try {
            // No per-job endpoint, so list all and filter to this job.
            const res = await client.get('/FollowUpReminder/list')
            setReminders(res.data.filter((r) => r.jobId === Number(jobId)))
        } catch {
            setError('Could not load reminders.')
        }
    }

    useEffect(() => {
        pull()
    }, [jobId])

    async function addReminder(e) {
        e.preventDefault()
        setError('')
        try {
            await client.post('/FollowUpReminder', {
                jobId: Number(jobId),
                followUpReminderTitle: title,
                followUpReminderNotes: notes,
                followUpReminderDate: date,
            })
            setTitle('')
            setNotes('')
            setDate(new Date().toISOString().slice(0, 10))
            pull()
        } catch {
            setError('Could not add the reminder.')
        }
    }

    async function complete(id) {
        try {
            await client.patch(`/FollowUpReminder/${id}/complete`)
            pull()
        } catch {
            setError('Could not update the reminder.')
        }
    }

    async function remove(id) {
        if (!window.confirm('Delete this reminder?')) return
        try {
            await client.delete(`/FollowUpReminder/${id}`)
            pull()
        } catch {
            setError('Could not delete the reminder.')
        }
    }

    return (
        <div className="mt-4 rounded-xl border border-gray-200 bg-white p-6 dark:border-gray-700 dark:bg-gray-800">
            <h3 className="mb-3 text-sm font-semibold text-gray-900 dark:text-gray-100">Reminders</h3>

            {error && (
                <div className="mb-3 rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700 dark:border-red-900 dark:bg-red-950 dark:text-red-300">{error}</div>
            )}

            {reminders.length > 0 && (
                <div className="mb-4 space-y-2">
                    {reminders.map((r) => (
                        <div key={r.followUpReminderId} className="flex items-start gap-3 rounded-lg border border-gray-200 p-3 dark:border-gray-700">
                            <input
                                type="checkbox"
                                checked={r.isCompleted}
                                disabled={r.isCompleted}
                                onChange={() => complete(r.followUpReminderId)}
                                className="mt-1 h-4 w-4 accent-violet-600"
                                aria-label="Mark complete"
                            />
                            <div className="min-w-0 flex-1">
                                <div className={`text-sm font-medium ${r.isCompleted ? 'text-gray-400 line-through dark:text-gray-500' : 'text-gray-900 dark:text-gray-100'}`}>
                                    {r.followUpReminderTitle}
                                </div>
                                {r.followUpReminderNotes && (
                                    <div className="text-xs text-gray-500 dark:text-gray-400">{r.followUpReminderNotes}</div>
                                )}
                                <div className="text-xs text-gray-400 dark:text-gray-500">
                                    Due {new Date(r.followUpReminderDate).toLocaleDateString()}
                                </div>
                            </div>
                            <button
                                onClick={() => remove(r.followUpReminderId)}
                                className="text-xs font-medium text-red-600 hover:text-red-700 dark:text-red-400"
                            >
                                Delete
                            </button>
                        </div>
                    ))}
                </div>
            )}

            <form onSubmit={addReminder} className="space-y-2">
                <div className="flex gap-2">
                    <input
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                        placeholder="New reminder…"
                        className={inputClass}
                    />
                    <input
                        type="date"
                        value={date}
                        onChange={(e) => setDate(e.target.value)}
                        className="w-40 shrink-0 rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-violet-500 focus:outline-none focus:ring-1 focus:ring-violet-500 dark:border-gray-600 dark:bg-gray-900 dark:text-gray-100"
                    />
                    <button
                        type="submit"
                        className="shrink-0 rounded-md bg-violet-600 px-3 py-2 text-sm font-medium text-white hover:bg-violet-700"
                    >
                        Add
                    </button>
                </div>
                <input
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                    placeholder="Notes (optional)"
                    className={inputClass}
                />
            </form>
        </div>
    )
}
