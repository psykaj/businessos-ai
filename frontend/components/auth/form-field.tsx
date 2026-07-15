import { cn } from "@/lib/utils";

interface FormFieldProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
  id: string;
}

export function FormField({ label, error, id, className, ...props }: FormFieldProps) {
  return (
    <div className="flex flex-col gap-1.5">
      <label
        htmlFor={id}
        className="text-sm font-medium text-foreground"
      >
        {label}
      </label>
      <input
        id={id}
        className={cn(
          "h-11 w-full rounded-lg border border-input bg-background px-3.5 text-sm text-foreground",
          "placeholder:text-muted-foreground",
          "outline-none ring-offset-background transition-all duration-150",
          "focus:border-primary focus:ring-2 focus:ring-primary/20",
          error && "border-destructive focus:ring-destructive/20",
          className
        )}
        {...props}
      />
      {error && (
        <p className="text-xs font-medium text-destructive">{error}</p>
      )}
    </div>
  );
}
