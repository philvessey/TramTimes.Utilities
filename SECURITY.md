# Security Policy

## 🐛 Reporting a Vulnerability

We take the security of TramTimes.Utilities seriously. If you believe you have found a security vulnerability,
please report it to us as described below.

### Please Do Not

- **Do not** open a public GitHub issue for security vulnerabilities
- **Do not** discuss the vulnerability in public forums, social media, or mailing lists

### How to Report

You can use GitHub's private vulnerability reporting feature:

1. Navigate to the repository's **Security** tab
2. Click **Report a vulnerability**
3. Fill out the vulnerability details form

### What to Include

To help us better understand and resolve the issue, please include as much of the following information as possible:

- **Type of vulnerability** (e.g., SQL injection, cross-site scripting, etc.)
- **Full paths of source file(s)** related to the vulnerability
- **Location of the affected source code** (tag/branch/commit or direct URL)
- **Step-by-step instructions to reproduce the issue**
- **Proof-of-concept or exploit code** (if possible)
- **Impact of the issue**, including how an attacker might exploit it

### What to Expect

After you submit a report, here's what you can expect:

- **Acknowledgment**: We will acknowledge receipt of your vulnerability report within **48 hours**
- **Initial Assessment**: We will provide an initial assessment within **5 business days**
- **Updates**: We will keep you informed about our progress as we work on a fix
- **Resolution**: We aim to release a fix within **30 days** for critical vulnerabilities
- **Credit**: With your permission, we will acknowledge your contribution in the release notes

## 🛡️ Security Best Practices

### For Contributors

When contributing to this project, please follow these security guidelines:

#### Code Security

- Never commit sensitive information (API keys, passwords, tokens, etc.)
- Use parameterized queries to prevent SQL injection
- Validate and sanitize all user inputs
- Keep dependencies up to date
- Follow the principle of least privilege

#### Dependency Management

- Regularly update NuGet packages to patch known vulnerabilities
- Run `dotnet list package --vulnerable` to check for vulnerable dependencies
- Review dependency updates for security advisories

#### Secrets Management

- Never hardcode credentials in source code
- Use environment variables or secure configuration providers
- Add sensitive file patterns to `.gitignore`
- Use .NET Secret Manager for local development: `dotnet user-secrets`

### For Users

When using these utilities:

#### Generated Code

- **Review generated code** before using it in production
- Ensure generated jobs have proper error handling
- Validate that connection strings and credentials are properly secured
- Use secure configuration management (Azure Key Vault, AWS Secrets Manager, etc.)

#### Database Security

- Use strong, unique passwords for database connections
- Implement proper network security (firewalls, VPNs)
- Enable SSL/TLS for database connections
- Follow the principle of least privilege for database users

#### Redis Security

- Enable Redis authentication with strong passwords
- Use SSL/TLS for Redis connections
- Restrict Redis network access
- Regularly update Redis to the latest secure version

#### Elasticsearch Security

- Enable Elasticsearch security features
- Use strong authentication
- Implement role-based access control
- Enable SSL/TLS for all connections
- Keep Elasticsearch updated to the latest secure version

## 🔐 Security Features

This project implements the following security measures:

- **Input Validation**: Data files are validated before processing
- **Error Handling**: Comprehensive error handling prevents information leakage
- **Dependency Management**: Centralized package management with regular updates
- **Code Quality**: Following .NET security best practices

## 📊 Security Updates

Security updates and patches will be released as needed:

- **Critical vulnerabilities**: Patched immediately
- **High severity**: Patched within 7 days
- **Medium severity**: Patched within 30 days
- **Low severity**: Patched in next regular release

Updates will be announced via:

- GitHub Security Advisories
- Release notes
- Repository notifications

## 🔍 Vulnerability Disclosure Policy

We follow a **coordinated disclosure** approach:

1. **Day 0**: Vulnerability reported to maintainers
2. **Day 0-2**: Acknowledgment sent to reporter
3. **Day 0-5**: Initial assessment and validation
4. **Day 5-30**: Develop and test fix
5. **Day 30**: Public disclosure and patch release (or sooner for critical issues)

## 🙏 Acknowledgments

We appreciate the security research community's efforts to responsibly disclose vulnerabilities. Contributors who report
valid security issues will be acknowledged in our release notes (unless they prefer to remain anonymous).

## 📚 Additional Resources

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [.NET Security Best Practices](https://docs.microsoft.com/en-us/dotnet/standard/security/)
- [GitHub Security Best Practices](https://docs.github.com/en/code-security)
- [NuGet Package Security](https://docs.microsoft.com/en-us/nuget/concepts/security-best-practices)

---

**Last Updated**: December 8, 2025