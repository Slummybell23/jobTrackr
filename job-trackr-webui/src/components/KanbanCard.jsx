import { useNavigate } from 'react-router-dom'
import StatusBadge from './ui/StatusBadge.jsx'
import { useDraggable } from '@dnd-kit/core'

export default function KanbanCard({ job }) {
    const navigate = useNavigate()

    const { attributes, listeners, setNodeRef, transform, isDragging } = useDraggable({ id: job.jobId })

    const style = transform
        ? { transform: `translate(${transform.x}px, ${transform.y}px)`, opacity: isDragging ? 0.5 : 1 }
        : undefined

    return (
        <div
            ref={setNodeRef}
            style={style}
            {...listeners}
            {...attributes}
            onClick={() => navigate(`/applications/${job.jobId}`)}
            className="cursor-pointer rounded-lg border border-gray-200 bg-white p-3 hover:border-violet-300 hover:shadow-sm dark:border-gray-700 dark:bg-gray-800 dark:hover:border-violet-500"
        >
            <div className="text-sm font-medium text-gray-900 dark:text-gray-100">{job.jobCompanyName}</div>
            <div className="mt-0.5 text-xs text-gray-500 dark:text-gray-400">{job.jobName}</div>

            {job.jobTags?.length > 0 && (
                <div className="mt-1.5 flex flex-wrap gap-1">
                    {job.jobTags.map((tag) => (
                        <span key={tag} className="rounded-full bg-gray-100 px-1.5 py-0.5 text-[11px] text-gray-600 dark:bg-gray-700 dark:text-gray-300">{tag}</span>
                    ))}
                </div>
            )}

            <div className="mt-2 flex items-center justify-between gap-2">
                <span className="text-xs text-gray-400 dark:text-gray-500">{job.jobAppliedDate}</span>
                <StatusBadge status={job.jobApplicationStatus} />
            </div>
        </div>
    )
}
