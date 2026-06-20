import KanbanCard from './KanbanCard.jsx'
import { statusStyle } from './ui/StatusBadge.jsx'
import { useDroppable } from '@dnd-kit/core'

export default function KanbanColumn({ title, jobs }) {
    const { setNodeRef, isOver } = useDroppable({ id: title })

    return (
        <div
            ref={setNodeRef}
            className={`flex w-64 flex-shrink-0 flex-col gap-2 rounded-xl bg-gray-100 p-2 dark:bg-gray-800/50 ${isOver ? 'ring-2 ring-violet-400' : ''}`}
        >
            <div className="flex items-center justify-between px-2 py-1 text-sm font-medium text-gray-700 dark:text-gray-300">
                <span className="flex items-center gap-2">
                    <span className={`h-2 w-2 rounded-full ${statusStyle(title).dot}`} />
                    {title}
                </span>
                <span className="rounded-full bg-gray-200 px-2 text-xs text-gray-600 dark:bg-gray-700 dark:text-gray-300">{jobs.length}</span>
            </div>

            {jobs.length === 0 ? (
                <div className="rounded-lg border border-dashed border-gray-300 px-3 py-6 text-center text-xs text-gray-400 dark:border-gray-700 dark:text-gray-500">
                    No applications
                </div>
            ) : (
                jobs.map((job) => <KanbanCard key={job.jobId} job={job} />)
            )}
        </div>
    )
}
