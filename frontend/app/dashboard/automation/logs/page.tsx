import { redirect } from "next/navigation";

export default function LogsPage() {
  // Logs are grouped with history in the new UI, redirect to history
  redirect("/dashboard/automation/history");
}
