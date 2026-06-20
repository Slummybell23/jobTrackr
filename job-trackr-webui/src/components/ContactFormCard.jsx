import { useState, useEffect } from 'react'
import client from '../api/client.js'
import Modal from './ui/Modal.jsx'

const inputClass = "w-full rounded-md border border-gray-300 px-3 py-2 text-sm focus:border-violet-500 focus:outline-none focus:ring-1 focus:ring-violet-500 dark:border-gray-600 dark:bg-gray-900 dark:text-gray-100 dark:placeholder-gray-500"

// One modal for both add and edit: pass a `contact` to edit, omit it to create.
export default function ContactFormCard({ contact, isOpen, onClose, onSaved }) {
    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')
    const [email, setEmail] = useState('')
    const [company, setCompany] = useState('')
    const [phone, setPhone] = useState('')
    const [notes, setNotes] = useState('')
    const [error, setError] = useState('')

    const isEdit = Boolean(contact)

    // Prefill from the contact when editing; clear when adding. Re-runs each time the modal opens.
    useEffect(() => {
        if (!isOpen) return
        setFirstName(contact?.contactFN ?? '')
        setLastName(contact?.contactLN ?? '')
        setEmail(contact?.contactEmail ?? '')
        setCompany(contact?.contactCompany ?? '')
        setPhone(contact?.contactPhoneNumber ?? '')
        setNotes(contact?.contactNotes ?? '')
        setError('')
    }, [isOpen, contact])

    async function handleSubmit(e) {
        e.preventDefault()
        setError('')

        const body = {
            contactFN: firstName,
            contactLN: lastName,
            contactEmail: email,
            contactCompany: company,
            contactPhoneNumber: phone,
            contactNotes: notes,
        }

        try {
            if (isEdit) {
                await client.put('/Contact/' + contact.contactId, body)
            } else {
                await client.post('/Contact', body)
            }
            onSaved()
            onClose()
        } catch {
            setError('Could not save contact.')
        }
    }

    return (
        <Modal isOpen={isOpen} onClose={onClose}>
            <h3 className="mb-4 text-lg font-semibold text-gray-900 dark:text-gray-100">{isEdit ? 'Edit contact' : 'Add contact'}</h3>

            <form onSubmit={handleSubmit} className="space-y-3">
                {error && (
                    <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">{error}</div>
                )}

                <div className="flex gap-3">
                    <div className="flex-1">
                        <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">First name</label>
                        <input value={firstName} onChange={(e) => setFirstName(e.target.value)} required className={inputClass} />
                    </div>
                    <div className="flex-1">
                        <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Last name</label>
                        <input value={lastName} onChange={(e) => setLastName(e.target.value)} required className={inputClass} />
                    </div>
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Email</label>
                    <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Company</label>
                    <input value={company} onChange={(e) => setCompany(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Phone</label>
                    <input value={phone} onChange={(e) => setPhone(e.target.value)} className={inputClass} />
                </div>
                <div>
                    <label className="mb-1 block text-sm font-medium text-gray-700 dark:text-gray-300">Notes</label>
                    <textarea rows={2} value={notes} onChange={(e) => setNotes(e.target.value)} className={inputClass} />
                </div>

                <div className="flex justify-end gap-2 pt-2">
                    <button type="button" onClick={onClose} className="rounded-md border border-gray-300 px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-700">
                        Cancel
                    </button>
                    <button type="submit" className="rounded-md bg-violet-600 px-4 py-2 text-sm font-medium text-white hover:bg-violet-700">
                        {isEdit ? 'Save changes' : 'Add contact'}
                    </button>
                </div>
            </form>
        </Modal>
    )
}
