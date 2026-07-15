import Link from "next/link";

interface AuthFormWrapperProps {
  title: string;
  subtitle: string;
  children: React.ReactNode;
  footer?: {
    text: string;
    linkText: string;
    href: string;
  };
}

export function AuthFormWrapper({
  title,
  subtitle,
  children,
  footer,
}: AuthFormWrapperProps) {
  return (
    <div className="w-full max-w-md">
      {/* Brand */}
      <div className="mb-8 flex flex-col items-center gap-3">
        <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-primary shadow-lg">
          <span className="text-lg font-bold text-primary-foreground">B</span>
        </div>
        <div className="text-center">
          <h1 className="text-2xl font-bold tracking-tight text-foreground">
            {title}
          </h1>
          <p className="mt-1 text-sm text-muted-foreground">{subtitle}</p>
        </div>
      </div>

      {/* Card */}
      <div className="rounded-2xl border border-border bg-card p-8 shadow-sm">
        {children}
      </div>

      {/* Footer link */}
      {footer && (
        <p className="mt-6 text-center text-sm text-muted-foreground">
          {footer.text}{" "}
          <Link
            href={footer.href}
            className="font-medium text-primary underline-offset-4 hover:underline"
          >
            {footer.linkText}
          </Link>
        </p>
      )}
    </div>
  );
}
